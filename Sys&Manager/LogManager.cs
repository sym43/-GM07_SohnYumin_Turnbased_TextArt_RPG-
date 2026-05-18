using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct LogMessage
{
    public string timeStamp;
    public string text;
    /// <summary>
    /// 고유 uid. uid가 같으면 같은 이벤트에서 같은 틱에 발생함.<br/>
    /// uid는 랜덤값은 아니고 그냥 흘러간 시간을 받아서 저장.<br/>
    /// </summary>
    public int groupId;
    public string highlightWord;
    public Color highlightColor;
    public LogMessage(string text, TimeSpan elapsedTime, int groupId, string highlightWord = "", Color highlightColor = default)
    {
        this.text = text;
        this.timeStamp = $"[ {elapsedTime.Hours:D2} : {elapsedTime.Minutes:D2} : {elapsedTime.Seconds:D2} ]";
        this.groupId = groupId;
        this.highlightWord = highlightWord;
        this.highlightColor = highlightColor;
    }
}
public class LogManager
{
    public static LogManager inst { get; private set; } = new LogManager();
    private Queue<LogMessage> _logQueue = new Queue<LogMessage>();
    private readonly int _maxLogLines = 10;
    private DateTime _startTime;
    private ScreenSurface _logView;
    private int _currentGroupId = 0;
    private LogManager() { }
    public void Init(ScreenSurface logview)
    {
        _logView = logview;
        _startTime = DateTime.Now;
        RenderLogView();
    }
    public void StartNewEvent()
    {
        _currentGroupId++;
    }
    public void AddLog(string msg)
    {
        if (_logQueue.Count >= _maxLogLines)
        {
            _logQueue.Dequeue(); // 가장 오래된 로그 제거
        }
        TimeSpan elapsedTime = DateTime.Now - _startTime;
        _logQueue.Enqueue(new LogMessage(msg, elapsedTime, _currentGroupId));
        RenderLogView();
    }
    /// <summary>
    /// 아이템용 로그
    /// </summary>
    /// <param name="item"></param>
    /// <param name="uid"></param>
    public void AddLog(Item item, string eventText)
    {
        if (_logQueue.Count >= _maxLogLines)
        {
            _logQueue.Dequeue(); // 가장 오래된 로그 제거
        }
        string msg = $"You {eventText} [ {item.name} ]";
        TimeSpan elapsedTime = DateTime.Now - _startTime;
        _logQueue.Enqueue(new LogMessage(msg, elapsedTime, _currentGroupId, item.name, item.tierColor[item.tier]));
        RenderLogView();
    }
    public void RenderLogView()
    {
        _logView.Surface.Clear(new Rectangle(1, 1, _logView.Width - 2, _logView.Height - 2));
        // 외곽선 제외 지우기

        int currentLine = 1;
        var logArray = _logQueue.ToArray();

        
        if (logArray.Length == 0) return;
        // 로그가 하나도 없다면 그릴 필요 없음

        int latestGroupId = logArray[logArray.Length - 1].groupId;
        for (int i = 0; i < logArray.Length; i++)
        {
            bool isLatestBatch = (logArray[i].groupId == latestGroupId);
            Color textColor = isLatestBatch ? Color.Yellow : Color.Gray;

            // 시간 정보와 텍스트 결합하여 출력
            string fullLog = $"{logArray[i].timeStamp} {logArray[i].text}";

            // 기본값인 ""거나 null일때
            if (string.IsNullOrEmpty(logArray[i].highlightWord))
            {
                _logView.Print(2, currentLine++, fullLog, textColor);
            }
            // 강조할 텍스트 일경우
            else
            {
                int textIndex = fullLog.IndexOf(logArray[i].highlightWord);
                if(textIndex == -1)
                {
                    _logView.Print(2, currentLine++, fullLog, textColor);
                }
                else
                {
                    // 앞뒤로 자르기
                    string part1 = fullLog.Substring(0, textIndex);
                    string part2 = logArray[i].highlightWord;
                    string part3 = fullLog.Substring(textIndex + part2.Length);
                    // 글자 커서
                    int currentX = 2;
                    _logView.Print(currentX, currentLine, part1, textColor);
                    currentX += part1.Length; // 그린 글자 수만큼 X 좌표 이동

                    _logView.Print(currentX, currentLine, part2, logArray[i].highlightColor);
                    currentX += part2.Length;

                    _logView.Print(currentX, currentLine, part3, textColor);
                    currentLine++;
                }
            }
        }
    }
}