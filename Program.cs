using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadConsole.Configuration;


Settings.WindowTitle = "RPG Game - Title";


int cellWidth = 160;
int cellHeight = 45;

Builder.GetBuilder()
    .SetWindowSizeInCells(cellWidth, cellHeight)
    .ConfigureFonts(true)
    .OnStart((host, config) =>
    {
        GameManager.inst.StartProgram();
    })
    .Run();
