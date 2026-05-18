using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BattleManager
{
    public Hero player { get; private set; }
    public Monster enemy { get; private set; }
    public bool isTurnProgress { get; private set; } = false;
    public bool isBattleEnd { get; private set; } = false;
    public static BattleManager inst { get; private set; } = new BattleManager();
    public BattleManager() { }
    public void EnterBattle(Hero player, Monster enemy)
    {
        this.player = player;
        this.enemy = enemy;
        isBattleEnd = false;
        enemy.DecideNextSkill();

        //ui 띄우기
        GameManager.inst.mainScreen.battleView.InitWhenStartBattle(player, enemy);
        GameManager.inst.mainScreen.battleView.OpenView();
    }
    public async Task StartTurn(Skill playerSkill)
    {
        player.DecideNextSkill((ActiveSkill)playerSkill);
        isTurnProgress = true;
        GameManager.inst.mainScreen.battleView.Render();
        bool playerStunned = player.isStunned;
        bool enemyStunned = enemy.isStunned;
        int delayTime = 1000;
        // 스턴 체크 단계.
        LogManager.inst.StartNewEvent();
        if (playerStunned)
        {
            LogManager.inst.AddLog($"{player.name} is stunned! Can't Move");
            await Task.Delay(delayTime);
        }
        if (enemyStunned)
        {
            LogManager.inst.AddLog($"{enemy.name} is stunned! Can't Move");
            await Task.Delay(delayTime);
        }

        // 방어 단계.
        if (!playerStunned && player.nextSkill.name.Contains("Defend"))
        {
            LogManager.inst.StartNewEvent();
            player.nextSkill.Use(player, player);
            await Task.Delay(delayTime);
        }
        if (!enemyStunned && enemy.nextSkill.name.Contains("Defend"))
        {
            LogManager.inst.StartNewEvent();
            enemy.nextSkill.Use(enemy, enemy);
            await Task.Delay(delayTime);
        }

        // 턴 시작때 적용되는 버프 실행. 틱뎀, 스탯 증가 등.
        LogManager.inst.StartNewEvent();
        player.TickEffects(true);
        enemy.TickEffects(true);
        GameManager.inst.mainScreen.battleView.Render();
        await Task.Delay(delayTime);

        // 공격 주고받기.
        if (!playerStunned && !player.isDead && !player.nextSkill.name.Contains("Defend"))
        {
            LogManager.inst.StartNewEvent();
            player.nextSkill.Use(enemy, player);
            GameManager.inst.mainScreen.battleView.Render();
            await Task.Delay(delayTime);
        }
        if (!enemyStunned && !enemy.isDead && !enemy.nextSkill.name.Contains("Defend"))
        {
            LogManager.inst.StartNewEvent();
            enemy.nextSkill.Use(player, enemy);
            GameManager.inst.mainScreen.battleView.Render();
            await Task.Delay(delayTime);
        }

        // 턴 끝날 때 틱감소시킴. 방어 스킬 썻을때, 턴 끝나고 방어력 돌려놓기 위함.
        LogManager.inst.StartNewEvent();
        player.TickEffects(false);
        enemy.TickEffects(false);

        // 턴 끝.
        LogManager.inst.StartNewEvent();
        LogManager.inst.AddLog("==========[ Turn end ]==========");
        isTurnProgress = false;
        if (player.isDead)
        {
            isBattleEnd = true;
            player.OnDead();
            Lose();
        }
        else if (enemy.isDead)
        {
            isBattleEnd = true;
            enemy.OnDead();
            Win();
        }
        else
        {
            enemy.DecideNextSkill();
        }
        GameManager.inst.mainScreen.battleView.Render();
    }

    // 둘다 배틀 뷰 창 꺼야함
    public void Win()
    {
        // 경험치 얻게 해야함.
        // 랜덤 아이템 드랍하게 해야함.
        GameManager.inst.mainScreen.battleView.CloseView();
        LogManager.inst.StartNewEvent();
        LogManager.inst.AddLog("You Win!!");
        GameObject removeObj = null;
        foreach (GameObject obj in GameManager.inst.currentMap.mapObjects)
        {
            // 그위치에 있는 gameobject 지우기
            if (obj is Enemy enemyObj)
            {
                if(enemyObj.monster == enemy)
                {
                    removeObj = obj;
                    
                }
            }
        }
        if(removeObj != null)
        {
            GameManager.inst.currentMap.mapObjects.Remove(removeObj);
        }
        GameManager.inst.currentMap.Draw();
    }
    public void Lose()
    {
        // 졌으니, 홈으로 돌아가고, 체력 초기화 해줘야 함.
        GameManager.inst.mainScreen.battleView.CloseView();
        LogManager.inst.StartNewEvent();
        LogManager.inst.AddLog("You Lose... Respawn to Home");
        GameManager.inst.LoadMap("Home");
        player.Respawn();
    }
}