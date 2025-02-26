using System.Threading;
namespace SudokuSolver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //define total Field
            Dictionary<string, string[]> totalField = new Dictionary<string, string[]>();
            //totalField.Add("row1", new string[9] { "1", "2", "3", "4", "5", "6", "7", "8", "9" });
            //totalField.Add("row2", new string[9] { "1", "2", "3", "4", "5", "6", "7", "8", "9" });
            //totalField.Add("row3", new string[9] { "1", "2", "3", "4", "5", "6", "7", "8", "9" });
            //totalField.Add("row4", new string[9] { "1", "2", "3", "4", "5", "6", "7", "8", "9" });
            //totalField.Add("row5", new string[9] { "1", "2", "3", "4", "5", "6", "7", "8", "9" });
            //totalField.Add("row6", new string[9] { "1", "2", "3", "4", "5", "6", "7", "8", "9" });
            //totalField.Add("row7", new string[9] { "1", "2", "3", "4", "5", "6", "7", "8", "9" });
            //totalField.Add("row8", new string[9] { "1", "2", "3", "4", "5", "6", "7", "8", "9" });
            //totalField.Add("row9", new string[9] { "1", "2", "3", "4", "5", "6", "7", "8", "9" });

            /*totalField.Add("row1", new string[9] { "", "", "", "", "", "", "", "", "" });
            totalField.Add("row2", new string[9] { "", "", "", "", "", "", "", "", "" });
            totalField.Add("row3", new string[9] { "", "", "", "", "", "", "", "", "" });
            totalField.Add("row4", new string[9] { "", "", "", "", "", "", "", "", "" });
            totalField.Add("row5", new string[9] { "", "", "", "", "", "", "", "", "" });
            totalField.Add("row6", new string[9] { "", "", "", "", "", "", "", "", "" });
            totalField.Add("row7", new string[9] { "", "", "", "", "", "", "", "", "" });
            totalField.Add("row8", new string[9] { "", "", "", "", "", "", "", "", "" });
            totalField.Add("row9", new string[9] { "", "", "", "", "", "", "", "", "" });*/

            totalField.Add("row1", new string[9] { "", "", "3", "4", "", "", "6", "", "" });
            totalField.Add("row2", new string[9] { "5", "", "", "", "2", "", "", "3", "" });
            totalField.Add("row3", new string[9] { "", "", "", "", "8", "", "", "", "9" });
            totalField.Add("row4", new string[9] { "", "1", "7", "", "", "", "", "", "" });
            totalField.Add("row5", new string[9] { "", "", "4", "", "6", "2", "", "", "" });
            totalField.Add("row6", new string[9] { "", "", "", "", "", "9", "", "", "" });
            totalField.Add("row7", new string[9] { "", "7", "", "", "", "", "5", "", "8" });
            totalField.Add("row8", new string[9] { "9", "", "", "6", "", "", "", "", "" });
            totalField.Add("row9", new string[9] { "", "", "", "", "4", "", "1", "", "7" });

            ShowField(totalField);

            int countFilledCells = CountFilledCells(totalField);
            int lastRoundFilledCells = countFilledCells;

            //define progress Field
            // gleiches Dictionary wie oben nur im Array ist pro Zelle noch ein Array mit für die Zelle möglichen Zahlen
            Dictionary<string, List<string>[]> progressField = CreateProgressField(totalField);

            bool firstRound = true;
            int countRoundsWithoutNumber = 0;

            //durchläuft die Reihen bis alle Zellen gefüllt sind

            //for (int i = 0; i < 1;  i++)
            Console.WriteLine("Click any key to start");
            Console.ReadKey();
            while (countFilledCells < 81)
            //for (int i = 0; i <= 20;  i++)
            {
                //durchläuft das Sudoku und bestimmt für alle Zellen die möglichen Zahlen
                (firstRound, countFilledCells, totalField, progressField) = GetAllPossibleNumbersInSudoku(firstRound, countFilledCells, totalField, progressField);
                /*WriteAllPossibleNumbers(progressField);
                Console.ReadKey();*/

                if (!firstRound)
                {
                    if (lastRoundFilledCells == countFilledCells)
                    {
                        countRoundsWithoutNumber++;
                    }
                    else
                    {
                        countRoundsWithoutNumber = 0;
                    }
                }

                //wenn mehrfach hintereinander keine Zahl hinzugefügt wird
                if (countRoundsWithoutNumber > 5)
                {
                    if (TryAnNumber(countFilledCells, totalField, progressField))
                    {
                        break;
                    } else
                    {
                        Console.WriteLine("Fehler");
                        break;
                    }
                }
                lastRoundFilledCells = countFilledCells;
                firstRound = false;
            }
            ShowField(totalField);
            Console.WriteLine("Click any key to end");
            Console.ReadKey();
        }

        static (bool, int, Dictionary<string, string[]>, Dictionary<string, List<string>[]>) GetAllPossibleNumbersInSudoku(bool firstRound, int countFilledCells, Dictionary<string, string[]> totalField, Dictionary<string, List<string>[]> progressField)
        {
            //durchläft die Reihen im Feld
            for (int rowNum = 0; rowNum < totalField.Count; rowNum++)
            {
                //durchläuft die Zelen in der Reihe
                for (int cellNum = 0; cellNum < totalField[$"row{rowNum + 1}"].Length; cellNum++)
                //for (int cellNum = 0; cellNum < totalField[$"row6"].Length; cellNum++)
                {
                    //überprüft ob die aktuelle Zelle schon befüllt ist
                    if (totalField[$"row{rowNum + 1}"][cellNum].Length == 0)
                    //if (totalField[$"row6"][cellNum].Length == 0)
                    {
                        List<string> posibleNumbersList = new List<string>();
                        //durchläuft die Zahlen 1 bis 9
                        for (int possibleNumber = 1; possibleNumber <= 9; possibleNumber++)
                        {
                            bool checkIfNumberIsInRow = CheckIfNumberIsInRow(possibleNumber.ToString(), totalField[$"row{rowNum + 1}"]);
                            bool checkIfNumberIsInColumn = CheckIfNumberIsInColumn(possibleNumber.ToString(), totalField, cellNum);
                            bool checkIfNumberIsInQuadrant = CheckIfNumberIsInQuadrant(possibleNumber.ToString(), totalField, rowNum, cellNum);

                            //Console.WriteLine($"[{rowNum+1}, {cellNum+1}]: {!checkIfNumberIsInRow} -- {!checkIfNumberIsInColumn} -- {!checkIfNumberIsInQuadrant} -- {!checkIfSamePossibleNumbersInRow}  -- {checkIfSamePossibleNumbersInColumn}");

                            if (!checkIfNumberIsInRow && !checkIfNumberIsInColumn && !checkIfNumberIsInQuadrant)
                            {
                                if (!firstRound && !CheckIfSamePossibleNumbersInRow(possibleNumber.ToString(), progressField, rowNum, cellNum) && !CheckIfSamePossibleNumbersInColumn(possibleNumber.ToString(), progressField, rowNum, cellNum))
                                {
                                    //Console.WriteLine($"AddNumberToList: [{rowNum}, {cellNum}]: {possibleNumber}");
                                    posibleNumbersList.Add(possibleNumber.ToString());
                                }
                                else if (firstRound)
                                {
                                    posibleNumbersList.Add(possibleNumber.ToString());
                                }

                            }
                        }
                        //fügt die möglichen Zahlen zu progressField hinzu
                        if (progressField[$"row{rowNum + 1}"][cellNum].Count > posibleNumbersList.Count && !firstRound)
                        {
                            progressField[$"row{rowNum + 1}"][cellNum] = posibleNumbersList;
                        } else if (firstRound)
                        {
                            progressField[$"row{rowNum + 1}"][cellNum] = posibleNumbersList;
                        }

                        /*Console.Write($"row: {rowNum + 1} col: {cellNum + 1} possible: [");
                        foreach (string d  in posibleNumbersList)
                        {
                            Console.Write(d + ",");
                        }
                        Console.WriteLine();
                        Console.ReadKey();*/

                        //wenn nur eine mögliche Zahl für die Zelle, dann wird sie zu totalField hinzugefügt
                        if (posibleNumbersList.Count == 1)
                        {
                            totalField[$"row{rowNum + 1}"][cellNum] = posibleNumbersList[0];
                            countFilledCells++;
                            Console.WriteLine(countFilledCells);
                            Thread.Sleep(200);
                            ShowField(totalField);
                        }

                        //gibt die möglichen Zahlen der aktuellen Zelle aus
                        //WritePossibleNumberTest(posibleNumbersList, cellNum);
                    }
                }
            }
            /*Console.WriteLine("ProgressField1");
            Console.ReadKey();
            WriteAllPossibleNumbers(progressField);
            Console.ReadKey();*/
            
            if (!firstRound)
            {
                progressField = CheckNumberPairsInQuadrant(progressField);
                progressField = CheckPairInRow(totalField, progressField);
                progressField = CheckPairInColumn(totalField, progressField);
            }
            progressField = CheckOnlyPossibleNumbersPlaceInRow(totalField, progressField);

            progressField = CheckOnlyPossibleNumbersPlaceInColumn(totalField, progressField);

            

            //überprüfen ob in Quadranten mögliche Zahl nur einmal vorkommt
            for (int rowQuadrantStartIndex = 0; rowQuadrantStartIndex <= 6; rowQuadrantStartIndex += 3)
            {
                for (int colQuadrantStartIndex = 0; colQuadrantStartIndex <= 6; colQuadrantStartIndex += 3)
                {
                    for (int possibleNumber = 1; possibleNumber <= 9; possibleNumber++)
                    {
                        var (checkIfNumbersIsOnceInQuadrant, rowWithSearchNumber, columnWithSearchNumber) = CheckIfNumbersIsOnceInQuadrant(possibleNumber.ToString(), progressField, rowQuadrantStartIndex, colQuadrantStartIndex);
                        if (checkIfNumbersIsOnceInQuadrant && !CheckIfNumberIsInQuadrant(possibleNumber.ToString(), totalField, rowQuadrantStartIndex, colQuadrantStartIndex))
                        {
                            //Console.WriteLine($"row: {rowWithSearchNumber}, column: {columnWithSearchNumber}");
                            if (totalField[$"row{rowWithSearchNumber + 1}"][columnWithSearchNumber] == "")
                            {
                                totalField[$"row{rowWithSearchNumber + 1}"][columnWithSearchNumber] = possibleNumber.ToString();
                                progressField[$"row{rowWithSearchNumber + 1}"][columnWithSearchNumber].Clear();
                                progressField[$"row{rowWithSearchNumber + 1}"][columnWithSearchNumber].Add(possibleNumber.ToString());
                                countFilledCells++;
                                Console.WriteLine(countFilledCells);
                                Thread.Sleep(200);
                                ShowField(totalField);
                            }
                        }
                    }
                }
            }

            //überprüfen ob in der Reihe mögliche Zahl nur einmal vorkommt
            for (int rowIndex = 0; rowIndex < 9; rowIndex++)
            {
                for (int possibleNumber = 1; possibleNumber <= 9; possibleNumber++)
                {
                    int countSamePossibleNumberInRow = 0;
                    int rowWithPossibleNummberIndex = rowIndex;
                    int colWithPossibleNummberIndex = 0;
                    for (int columnIndex = 0; columnIndex < 9; columnIndex++)
                    {
                        if (totalField[$"row{rowIndex + 1}"][columnIndex] == "")
                        {
                            if (progressField[$"row{rowIndex + 1}"][columnIndex].Contains(possibleNumber.ToString()))
                            {
                                rowWithPossibleNummberIndex = rowIndex;
                                colWithPossibleNummberIndex = columnIndex;
                                countSamePossibleNumberInRow++;
                            }
                        }
                    }
                    //Console.WriteLine($"[{rowWithPossibleNummberIndex}, {colWithPossibleNummberIndex}], number: {possibleNumber} count: {countSamePossibleNumberInRow}");
                    //Console.ReadKey();
                    if (countSamePossibleNumberInRow == 1 && !CheckIfNumberIsInRow(possibleNumber.ToString(), totalField.ElementAt(rowWithPossibleNummberIndex).Value) && !CheckIfNumberIsInColumn(possibleNumber.ToString(), totalField, colWithPossibleNummberIndex))
                    {
                        totalField[$"row{rowWithPossibleNummberIndex + 1}"][colWithPossibleNummberIndex] = possibleNumber.ToString();
                        progressField[$"row{rowWithPossibleNummberIndex + 1}"][colWithPossibleNummberIndex].Clear();
                        progressField[$"row{rowWithPossibleNummberIndex + 1}"][colWithPossibleNummberIndex].Add(possibleNumber.ToString());
                        countFilledCells++;
                        //Console.WriteLine("Yeah");
                        Thread.Sleep(200);
                        ShowField(totalField);
                    }
                }

            }

            //überprüfen ob in der Spalte mögliche Zahl nur einmal vorkommt
            for (int colIndex = 0; colIndex < 9; colIndex++)
            {
                for (int possibleNumber = 1; possibleNumber <= 9; possibleNumber++)
                {
                    int countSamePossibleNumberInCol = 0;
                    int colWithPossibleNummberIndex = colIndex;
                    int rowWithPossibleNummberIndex = 0;
                    for (int rowIndex = 0; rowIndex < 9; rowIndex++)
                    {
                        if (totalField[$"row{rowIndex + 1}"][colIndex] == "")
                        {
                            if (progressField[$"row{rowIndex + 1}"][colIndex].Contains(possibleNumber.ToString()))
                            {
                                rowWithPossibleNummberIndex = rowIndex;
                                colWithPossibleNummberIndex = colIndex;
                                countSamePossibleNumberInCol++;
                            }
                        }
                    }
                    //Console.WriteLine($"[{rowWithPossibleNummberIndex}, {colWithPossibleNummberIndex}], number: {possibleNumber} count: {countSamePossibleNumberInRow}");
                    //Console.ReadKey();
                    if (countSamePossibleNumberInCol == 1 && !CheckIfNumberIsInRow(possibleNumber.ToString(), totalField.ElementAt(rowWithPossibleNummberIndex).Value) && !CheckIfNumberIsInColumn(possibleNumber.ToString(), totalField, colWithPossibleNummberIndex))
                    {
                        totalField[$"row{rowWithPossibleNummberIndex + 1}"][colWithPossibleNummberIndex] = possibleNumber.ToString();
                        progressField[$"row{rowWithPossibleNummberIndex + 1}"][colWithPossibleNummberIndex].Clear();
                        progressField[$"row{rowWithPossibleNummberIndex + 1}"][colWithPossibleNummberIndex].Add(possibleNumber.ToString());
                        countFilledCells++;
                        //Console.WriteLine("Yeah");
                        Thread.Sleep(200);
                        ShowField(totalField);
                    }
                }

            }

            return (firstRound, countFilledCells, totalField, progressField);
        }

        static void WritePossibleNumberTest(List<string> posibleNumbersList, int cellNum)
        {
            Console.Write($"{cellNum + 1}: ");
            foreach (string number in posibleNumbersList)
            {
                Console.Write(number + ", ");
            }
            Console.WriteLine();
        }

        static void WriteAllPossibleNumbers(Dictionary<string, List<string>[]> progressField)
        {
            for (int rowIndex = 1; rowIndex <= 9; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < 9; columnIndex++)
                {
                    Console.Write($"[{rowIndex}, {columnIndex + 1}]: ");
                    foreach (string number in progressField[$"row{rowIndex}"][columnIndex])
                    {
                        Console.Write(number + ", ");
                    }
                    Console.WriteLine();
                }
            }
        }
        static void ShowField(Dictionary<string, string[]> totalField)
        {
            for (int i = 0; i < 19; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine();
            foreach (KeyValuePair<string, string[]> row in totalField)
            {
                Console.Write("|");
                foreach (string cell in row.Value)
                {
                    if (cell == "")
                    {
                        Console.Write(" ");
                    } else
                    {
                        Console.Write(cell);
                    }
                    Console.Write("|");
                }
                Console.WriteLine();
            }
            for (int i = 0; i < 19; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine();
        }

        static int CountFilledCells(Dictionary<string, string[]> totalField)
        {
            int filledCellCount = 0;
            foreach (var row in totalField)
            {
                foreach (var cell in row.Value)
                {
                    if (cell != "")
                    {
                        filledCellCount++;
                    }
                }
            }
            return filledCellCount;
        }


        static Dictionary<string, List<string>[]> CreateProgressField(Dictionary<string, string[]> totalField)
        {
            Dictionary<string, List<string>[]> progressField = new Dictionary<string, List<string>[]>();
            //Liste im Array
            foreach (KeyValuePair<string, string[]> row in totalField)
            {
                List<string>[] posibleNumbers = new List<string>[9];
                for (int i = 0; i < row.Value.Length; i++) 
                {
                    List<string> posibleNumbersList = new List<string>();
                    posibleNumbersList.Add(row.Value[i]);
                    posibleNumbers[i] = posibleNumbersList;
                }
                progressField.Add(row.Key, posibleNumbers);
            }

            return progressField;
        }

        static bool CheckIfNumberIsInRow(string searchNumber, string[] row)
        {
            foreach (string number in row)
            {
                if (number == searchNumber)
                {
                    return true;
                }
            }
            return false;
        }

        static bool CheckIfNumberIsInColumn(string searchNumber, Dictionary<string, string[]> totalField, int currentColumn)
        {
            foreach (var row in totalField)
            {
                if (row.Value[currentColumn] == searchNumber)
                {
                    return true;
                }
            }
            return false;
        }

        static bool CheckIfNumberIsInQuadrant(string searchNumber, Dictionary<string, string[]> totalField, int currentRow, int currentColumn)
        {
            int rowStartIndex = 0;
            // wenn rowIndex % 3 = 0 dann startindex = rowIndex - 0
            // wenn rowIndex % 3 = 1 dann startindex = rowIndex - 1
            // wenn rowIndex % 3 = 2 dann startindex = rowIndex - 2
            switch (currentRow % 3)
            {
                case 0: rowStartIndex = currentRow; break;
                case 1: rowStartIndex = currentRow - 1; break;
                case 2: rowStartIndex = currentRow - 2; break;
                default: Console.WriteLine("RowQuadrantFailure"); break;
            }

            int columnStartIndex = 0;
            switch (currentColumn % 3)
            {
                case 0: columnStartIndex = currentColumn; break;
                case 1: columnStartIndex = currentColumn - 1; break;
                case 2: columnStartIndex = currentColumn - 2; break;
                default: Console.WriteLine("ColumnQuadrantFailure"); break;

            }

            for (int rowIndex = rowStartIndex; rowIndex < rowStartIndex + 3; rowIndex++)
            {
                for (int columnIndex = columnStartIndex; columnIndex < columnStartIndex + 3; columnIndex++)
                {
                    if (totalField.ElementAt(rowIndex).Value[columnIndex] == searchNumber)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        static (bool,int,int) CheckIfNumbersIsOnceInQuadrant(string searchNumber, Dictionary<string, List<string>[]> progressField, int startRow, int startColumn)
        {
            int countSearchNumber = 0;
            int rowWithSearchNumber = 0;
            int columnWithSearchNumber = 0;
            for (int rowIndex = startRow; rowIndex < startRow + 3; rowIndex++)
            {
                for (int columnIndex = startColumn; columnIndex < startColumn + 3;columnIndex++)
                {
                    if (progressField[$"row{rowIndex + 1}"][columnIndex].Contains(searchNumber))
                    {
                        countSearchNumber++;
                        rowWithSearchNumber = rowIndex;
                        columnWithSearchNumber = columnIndex;
                    }
                }
            }
            if (countSearchNumber == 1)
            {
                return (true, rowWithSearchNumber, columnWithSearchNumber);
            }
            return (false,0,0);
        }
        static bool CheckIfSamePossibleNumbersInRow(string searchNumber, Dictionary<string, List<string>[]> progressField, int currentRow, int currentColumn)
        {
            //Index der beiden Zeilen um die aktuelle Zeile bestimmen
            (int row1Index, int row2Index) = GetTwoOtherRowsInQuadrant(currentRow);
            
            //start Spalte der beiden anderen Quadranten bestimmen
            int quadrant1StartColumn = 0;
            int quadrant2StartColumn = 0;
            if (currentColumn < 3)
            {
                quadrant1StartColumn = 3;
                quadrant2StartColumn = 6;
            } else if (currentColumn < 6)
            {
                quadrant1StartColumn = 0;
                quadrant2StartColumn = 6;
            } else if (currentColumn < 9)
            {
                quadrant1StartColumn = 0;
                quadrant2StartColumn = 3;
            }

            int numberCountInQuadrant1 = 0;
            //Quadrant 1 überprüfen
            for (int quadrant1Index = quadrant1StartColumn; quadrant1Index < quadrant1StartColumn + 3; quadrant1Index++)
            {
                if (progressField[$"row{currentRow + 1}"][quadrant1Index].Contains(searchNumber))
                {
                    numberCountInQuadrant1++;
                }
            }

            //überprüfen ob Zahl nicht in anderen Reihen des Quadranten vorkommt
            int numberCountInQuadrant1Row1 = 0;
            for (int quadrant1Index = quadrant1StartColumn; quadrant1Index < quadrant1StartColumn + 3; quadrant1Index++)
            {
                if (progressField[$"row{row1Index + 1}"][quadrant1Index].Contains(searchNumber))
                {
                    numberCountInQuadrant1Row1++;
                }
            }

            int numberCountInQuadrant1Row2 = 0;
            for (int quadrant1Index = quadrant1StartColumn; quadrant1Index < quadrant1StartColumn + 3; quadrant1Index++)
            {
                if (progressField[$"row{row2Index + 1}"][quadrant1Index].Contains(searchNumber))
                {
                    numberCountInQuadrant1Row2++;
                }
            }

            //Console.WriteLine($"FirstIfRow: {searchNumber} -- {numberCountInQuadrant1}; {numberCountInQuadrant1Row1}; {numberCountInQuadrant1Row2}");
            if (numberCountInQuadrant1 > 1 && (numberCountInQuadrant1Row1 + numberCountInQuadrant1Row2) == 0)
            {
                return true;
            }

            int numberCountInQuadrant2 = 0;
            //Quadrant 2 überprüfen
            for (int quadrant2Index = quadrant2StartColumn; quadrant2Index < quadrant2StartColumn + 3; quadrant2Index++)
            {
                if (progressField[$"row{currentRow + 1}"][quadrant2Index].Contains(searchNumber))
                {
                    numberCountInQuadrant2++;
                }
            }

            //überprüfen ob Zahl nicht in anderen Reihen des Quadranten vorkommt
            int numberCountInQuadrant2Row1 = 0;
            for (int quadrant2Index = quadrant2StartColumn; quadrant2Index < quadrant2StartColumn + 3; quadrant2Index++)
            {
                if (progressField[$"row{row1Index + 1}"][quadrant2Index].Contains(searchNumber))
                {
                    numberCountInQuadrant2Row1++;
                }
            }

            int numberCountInQuadrant2Row2 = 0;
            for (int quadrant2Index = quadrant2StartColumn; quadrant2Index < quadrant2StartColumn + 3; quadrant2Index++)
            {
                if (progressField[$"row{row2Index + 1}"][quadrant2Index].Contains(searchNumber))
                {
                    numberCountInQuadrant2Row2++;
                }
            }

            //Console.WriteLine($"SecondIf: {numberCountInQuadrant2}; {numberCountInQuadrant2Row1}; {numberCountInQuadrant2Row2}");

            if (numberCountInQuadrant2 > 1 && (numberCountInQuadrant2Row1 + numberCountInQuadrant2Row2) == 0)
            {
                return true;
            }

            //Console.WriteLine("ThirdIf");
            if (numberCountInQuadrant1 >= 1 && numberCountInQuadrant2 >= 1 && numberCountInQuadrant1Row1 >= 1 && numberCountInQuadrant2Row1 >= 1 && numberCountInQuadrant1Row2 == 0 && numberCountInQuadrant2Row2 == 0)
            {
                return true;
            }

            //Console.WriteLine("ForthIf");

            if (numberCountInQuadrant1 >= 1 && numberCountInQuadrant2 >= 1 && numberCountInQuadrant1Row2 >= 1 && numberCountInQuadrant2Row2 >= 1 && numberCountInQuadrant1Row1 == 0 && numberCountInQuadrant2Row1 == 0)
            {
                return true;
            }

            //Console.WriteLine("NoneIf");

            return false;
        }

        static bool CheckIfSamePossibleNumbersInColumn(string searchNumber, Dictionary<string, List<string>[]> progressField, int currentRow, int currentCol)
        {
            //Index der beiden Zeilen um die aktuelle Zeile bestimmen
            int col1Index = 0;
            int col2Index = 0;
            switch (currentCol % 3)
            {
                case 0:
                    col1Index = currentCol + 1; //Zeile eins darunter
                    col2Index = currentCol + 2; //Zeile zwei darunter
                    break;
                case 1:
                    col1Index = currentCol - 1; //Zeile drüber
                    col2Index = currentCol + 1; //Zeile drunter
                    break;
                case 2:
                    col1Index = currentCol - 2; //Zeile zwei drüber
                    col2Index = currentCol - 1; //Zeile eins drüber
                    break;
                default: Console.WriteLine("RowQuadrantFailure"); break;
            }
            //start Zeile der beiden anderen Quadranten bestimmen
            int quadrant1StartRow = 0;
            int quadrant2StartRow = 0;
            if (currentRow < 3)
            {
                quadrant1StartRow = 3;
                quadrant2StartRow = 6;
            }
            else if (currentRow < 6)
            {
                quadrant1StartRow = 0;
                quadrant2StartRow = 6;
            }
            else if (currentRow < 9)
            {
                quadrant1StartRow = 0;
                quadrant2StartRow = 3;
            }

            int numberCountInQuadrant1 = 0;
            //Zahl in aktueller Spalte des Quadranten1 suchen
            for (int quadrant1RowIndex = quadrant1StartRow; quadrant1RowIndex < quadrant1StartRow + 3; quadrant1RowIndex++)
            {
                if (progressField[$"row{quadrant1RowIndex + 1}"][currentCol].Contains(searchNumber))
                {
                    numberCountInQuadrant1++;
                }
            }

            //überprüfen ob Zahl nicht in anderen Spalten des Quadranten1 vorkommt
            int numberCountInQuadrant1Col1 = 0;
            for (int quadrant1RowIndex = quadrant1StartRow; quadrant1RowIndex < quadrant1StartRow + 3; quadrant1RowIndex++)
            {
                if (progressField[$"row{quadrant1RowIndex + 1}"][col1Index].Contains(searchNumber))
                {
                    numberCountInQuadrant1Col1++;
                }
            }

            int numberCountInQuadrant1Col2 = 0;

            for (int quadrant1RowIndex = quadrant1StartRow; quadrant1RowIndex < quadrant1StartRow + 3; quadrant1RowIndex++)
            {
                if (progressField[$"row{quadrant1RowIndex + 1}"][col2Index].Contains(searchNumber))
                {
                    numberCountInQuadrant1Col2++;
                }
            }

            //Console.WriteLine($"[{currentRow}, {currentCol}]; {searchNumber}: Quadrant1 {quadrant1StartRow}, {numberCountInQuadrant1}, {numberCountInQuadrant1OtherRows}");

            //Console.WriteLine($"FirstIfCol: {searchNumber} -- {numberCountInQuadrant1}; {numberCountInQuadrant1Col1}; {numberCountInQuadrant1Col2}");

            if (numberCountInQuadrant1 > 1 && (numberCountInQuadrant1Col1 + numberCountInQuadrant1Col2) == 0)
            {
                return true;
            }

            
            int numberCountInQuadrant2 = 0;
            //Zahl in aktueller Spalte des Quadranten2 suchen
            for (int quadrant2RowIndex = quadrant2StartRow; quadrant2RowIndex < quadrant2StartRow + 3; quadrant2RowIndex++)
            {
                if (progressField[$"row{quadrant2RowIndex + 1}"][currentCol].Contains(searchNumber))
                {
                    numberCountInQuadrant2++;
                }
            }

            //überprüfen ob Zahl nicht in anderen Spalten des Quadranten2 vorkommt
            int numberCountInQuadrant2Col1 = 0;
            for (int quadrant2RowIndex = quadrant2StartRow; quadrant2RowIndex < quadrant2StartRow + 3; quadrant2RowIndex++)
            {
                if (progressField[$"row{quadrant2RowIndex + 1}"][col1Index].Contains(searchNumber))
                {
                    numberCountInQuadrant2Col1++;
                }
            }

            int numberCountInQuadrant2Col2 = 0;

            for (int quadrant2RowIndex = quadrant2StartRow; quadrant2RowIndex < quadrant2StartRow + 3; quadrant2RowIndex++)
            {
                if (progressField[$"row{quadrant2RowIndex + 1}"][col2Index].Contains(searchNumber))
                {
                    numberCountInQuadrant2Col2++;
                }
            }
            /*Console.WriteLine($"[{currentRow}, {currentCol}]; {searchNumber}: Quadrant1 {quadrant2StartRow}, {numberCountInQuadrant2}, {numberCountInQuadrant2OtherRows}");
            Console.ReadKey();*/
            //Console.WriteLine($"SecondIfCol: {searchNumber} -- {numberCountInQuadrant2}; {numberCountInQuadrant2Col1}; {numberCountInQuadrant2Col2}");

            if (numberCountInQuadrant2 > 1 && (numberCountInQuadrant2Col1 + numberCountInQuadrant2Col2) == 0)
            {
                return true;
            }

            //Console.WriteLine("ThirdIf");

            if (numberCountInQuadrant1 >= 1 && numberCountInQuadrant2 >= 1 && numberCountInQuadrant1Col1 >= 1 && numberCountInQuadrant2Col1 >= 1 && numberCountInQuadrant1Col2 == 0 && numberCountInQuadrant2Col2 == 0)
            {
                return true;
            }

            //Console.WriteLine("ForthIf");
            if (numberCountInQuadrant1 >= 1 && numberCountInQuadrant2 >= 1 && numberCountInQuadrant1Col2 >= 1 && numberCountInQuadrant2Col2 >= 1 && numberCountInQuadrant1Col1 == 0 && numberCountInQuadrant2Col1 == 0)
            {
                return true;
            }

            //Console.WriteLine("NoneIf");

            return false;
        }

        static Dictionary<string, List<string>[]> CheckNumberPairsInQuadrant(Dictionary<string, List<string>[]> progressField)
        {
            /*
             wenn es innerhalb eines Quadranten ein paar gibt zum Beispiel in zwei Zellen sind die möglichen Zahlen 2 und 8, können in den anderen Zellen keine 2 und 8 sein
             wenn paar nebeneinander bzw. untereinander ist kann auch in der Zeile bzw. Spalte keine 2 oder 8 sein
             */
            //überprüfen ob in Quadranten mögliche Zahl nur einmal vorkommt

            //go throgh every quadrant in the sudoku

            /*WriteAllPossibleNumbers(progressField);
            Console.WriteLine("_______________");
            Console.ReadKey();*/
            for (int rowQuadrantStartIndex = 1; rowQuadrantStartIndex <= 7; rowQuadrantStartIndex += 3)
            {
                for (int colQuadrantStartIndex = 0; colQuadrantStartIndex <= 6; colQuadrantStartIndex += 3)
                {
                    //int countPair = 0;
                    //go through every row in the quadrant
                    for (int rowIndex = rowQuadrantStartIndex; rowIndex < rowQuadrantStartIndex + 3; rowIndex++)
                    {
                        //go through every column in the quadrant
                        for (int columnIndex = colQuadrantStartIndex; columnIndex < colQuadrantStartIndex + 3; columnIndex++)
                        {
                            //check if there are only two possible numbers
                            /*if (progressField[$"row{rowIndex}"][columnIndex].Count == 2)
                            {
                                List<string> searchPair = new List<string>(progressField[$"row{rowIndex}"][columnIndex]);
                                //count how many times the pair is in the quadrant
                                for (int rowIndex2 = rowQuadrantStartIndex; rowIndex2 < rowQuadrantStartIndex + 3; rowIndex2++)
                                {
                                    for (int columnIndex2 = colQuadrantStartIndex; columnIndex2 < colQuadrantStartIndex + 3; columnIndex2++)
                                    {
                                        if (progressField[$"row{rowIndex2}"][columnIndex2].SequenceEqual(searchPair))
                                        {
                                            countPair++;
                                        }
                                    }
                                }
                                if (countPair == 2)
                                {
                                    //go through quadrant end delete the numbers in the pair from the other cells
                                    for (int rowIndex2 = rowQuadrantStartIndex; rowIndex2 < rowQuadrantStartIndex + 3; rowIndex2++)
                                    {
                                        for (int columnIndex2 = colQuadrantStartIndex; columnIndex2 < colQuadrantStartIndex + 3; columnIndex2++)
                                        {
                                            if (!progressField[$"row{rowIndex2}"][columnIndex2].SequenceEqual(searchPair) && (progressField[$"row{rowIndex2}"][columnIndex2].Contains(searchPair[0]) || progressField[$"row{rowIndex2}"][columnIndex2].Contains(searchPair[1])))
                                            {
                                                progressField[$"row{rowIndex2}"][columnIndex2].Remove(searchPair[0]);
                                                progressField[$"row{rowIndex2}"][columnIndex2].Remove(searchPair[1]);
                                            }
                                        }
                                    }
                                }
                                countPair = 0;
                            }*/
                            List<string> searchPair = new List<string>(progressField[$"row{rowIndex}"][columnIndex]);
                            for (int numIndex = 0; numIndex < searchPair.Count - 1; numIndex++)
                            {
                                for (int numIndex2 = 0; numIndex2 < searchPair.Count; numIndex2++)
                                {
                                    if (numIndex == numIndex2)
                                    {
                                        continue;
                                    }
                                    string num1 = searchPair[numIndex];
                                    string num2 = searchPair[numIndex2];
                                    int[] pairPlace1 = new int[2];
                                    int[] pairPlace2 = new int[2];
                                    int countPair = 0;
                                    for (int rowIndex2 = rowQuadrantStartIndex; rowIndex2 < rowQuadrantStartIndex + 3; rowIndex2++)
                                    {
                                        //go through every column in the quadrant
                                        for (int columnIndex2 = colQuadrantStartIndex; columnIndex2 < colQuadrantStartIndex + 3; columnIndex2++)
                                        {
                                            if (progressField[$"row{rowIndex2}"][columnIndex2].Contains(num1) && progressField[$"row{rowIndex2}"][columnIndex2].Contains(num2))
                                            {
                                                if (countPair == 0)
                                                {
                                                    pairPlace1[0] = rowIndex2;
                                                    pairPlace1[1] = columnIndex2;
                                                    countPair++;
                                                }
                                                else if (countPair == 1)
                                                {
                                                    pairPlace2[0] = rowIndex2;
                                                    pairPlace2[1] = columnIndex2;
                                                    countPair++;
                                                }
                                                else
                                                {
                                                    countPair++;
                                                }
                                            }
                                            else if ((!progressField[$"row{rowIndex2}"][columnIndex2].Contains(num1) && progressField[$"row{rowIndex2}"][columnIndex2].Contains(num2)) || progressField[$"row{rowIndex2}"][columnIndex2].Contains(num1) && !progressField[$"row{rowIndex2}"][columnIndex2].Contains(num2))
                                            {
                                                countPair = 5;
                                            }
                                        }
                                    }

                                    /*Console.WriteLine($"[{rowQuadrantStartIndex}, {colQuadrantStartIndex}]: {num1}, {num2}; {countPair} -- pairPlace1: [{pairPlace1[0]}, {pairPlace1[1]}], pairPlace2: [{pairPlace2[0]}, {pairPlace2[1]}]");
                                    Console.ReadKey();*/
                                    if (countPair == 2)
                                    {
                                        //Console.WriteLine("CountPair = 2");
                                        bool numCountInPair1Cell = progressField[$"row{pairPlace1[0]}"][pairPlace1[1]].Count == 2;
                                        bool numCountInPair2Cell = progressField[$"row{pairPlace2[0]}"][pairPlace2[1]].Count == 2;
                                        for (int rowIndex2 = rowQuadrantStartIndex; rowIndex2 < rowQuadrantStartIndex + 3; rowIndex2++)
                                        {
                                            //go through every column in the quadrant

                                            for (int columnIndex2 = colQuadrantStartIndex; columnIndex2 < colQuadrantStartIndex + 3; columnIndex2++)
                                            {
                                                if ((rowIndex2 == pairPlace1[0] && columnIndex2 == pairPlace1[1]) || (rowIndex2 == pairPlace2[0] && columnIndex2 == pairPlace2[1]))
                                                {
                                                    progressField[$"row{rowIndex2}"][columnIndex2].RemoveAll(item => item != num1 && item != num2);
                                                    /*Console.WriteLine("Remove numbers from PairCells");
                                                    Console.Write($"[{rowIndex2}, {columnIndex2}]: ");
                                                    foreach (string num in progressField[$"row{rowIndex2}"][columnIndex2])
                                                    {
                                                        Console.Write(num + ", ");
                                                    }
                                                    Console.WriteLine();*/
                                                }
                                                else if (numCountInPair1Cell && numCountInPair2Cell)
                                                {
                                                    progressField[$"row{rowIndex2}"][columnIndex2].Remove(num1);
                                                    progressField[$"row{rowIndex2}"][columnIndex2].Remove(num2);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return progressField;
        }

        static Dictionary<string, List<string>[]> CheckOnlyPossibleNumbersPlaceInRow(Dictionary<string, string[]> totalField, Dictionary<string, List<string>[]> progressField)
        {
            /*
             wenn in einer Reihe die möglichen Plätze für eine noch fehlende Zahl nur innerhalb eines Quadranten liegen, 
             können in den anderen Reihen des Quadranten nicht diese Zahl sein
             */
            for (int rowIndex = 1; rowIndex <= 9; rowIndex++)
            {
                int countNumberInQuadrant1 = 0;
                int countNumberInQuadrant2 = 0;
                int countNumberInQuadrant3 = 0;
                for (int searchNumber = 1; searchNumber <= 9; searchNumber++)
                {
                    if (CheckIfNumberIsInRow(searchNumber.ToString(), totalField[$"row{rowIndex}"]))
                    {
                        continue;
                    }
                    for (int colQuadrantStartIndex = 0; colQuadrantStartIndex <= 6; colQuadrantStartIndex += 3)
                    {
                        //go through every column in the quadrant
                        for (int columnIndex = colQuadrantStartIndex; columnIndex < colQuadrantStartIndex + 3; columnIndex++)
                        {
                            if (progressField[$"row{rowIndex}"][columnIndex].Contains(searchNumber.ToString()))
                            {
                                if (colQuadrantStartIndex == 0)
                                {
                                    countNumberInQuadrant1++;
                                }
                                else if (colQuadrantStartIndex == 3)
                                {
                                    countNumberInQuadrant2++;
                                }
                                else if (colQuadrantStartIndex == 6)
                                {
                                    countNumberInQuadrant3++;
                                }
                            }
                        }
                    }
                    if (countNumberInQuadrant1 != 0 && countNumberInQuadrant2 == 0 &&  countNumberInQuadrant3 == 0)
                    {
                        (int row1Index, int row2Index) = GetTwoOtherRowsInQuadrant(rowIndex-1);
                        for (int colIndex = 0; colIndex < 3; colIndex++)
                        {
                            progressField[$"row{row1Index + 1}"][colIndex].Remove(searchNumber.ToString());
                            progressField[$"row{row2Index + 1}"][colIndex].Remove(searchNumber.ToString());
                        }
                    } else if (countNumberInQuadrant1 == 0 && countNumberInQuadrant2 != 0 && countNumberInQuadrant3 == 0)
                    {
                        (int row1Index, int row2Index) = GetTwoOtherRowsInQuadrant(rowIndex - 1);
                        for (int colIndex = 3; colIndex < 6; colIndex++)
                        {
                            progressField[$"row{row1Index + 1}"][colIndex].Remove(searchNumber.ToString());
                            progressField[$"row{row2Index + 1}"][colIndex].Remove(searchNumber.ToString());
                        }
                    } else if (countNumberInQuadrant1 == 0 && countNumberInQuadrant2 == 0 && countNumberInQuadrant3 != 0)
                    {
                        (int row1Index, int row2Index) = GetTwoOtherRowsInQuadrant(rowIndex - 1);
                        for (int colIndex = 6; colIndex < 9; colIndex++)
                        {
                            progressField[$"row{row1Index + 1}"][colIndex].Remove(searchNumber.ToString());
                            progressField[$"row{row2Index + 1}"][colIndex].Remove(searchNumber.ToString());
                        }
                    }
                }
            }
            return progressField;
        }

        static Dictionary<string, List<string>[]> CheckOnlyPossibleNumbersPlaceInColumn(Dictionary<string, string[]> totalField, Dictionary<string, List<string>[]> progressField)
        {
            /*
             wenn in einer Spalte die möglichen Plätze für eine noch fehlende Zahl nur innerhalb eines Quadranten liegen, 
             können in den anderen Spalten des Quadranten nicht diese Zahl sein
            */
            for (int colIndex = 0; colIndex < 9; colIndex++)
            {
                int countNumberInQuadrant1 = 0;
                int countNumberInQuadrant2 = 0;
                int countNumberInQuadrant3 = 0;
                for (int searchNumber = 1; searchNumber <= 9; searchNumber++)
                {
                    if (CheckIfNumberIsInColumn(searchNumber.ToString(), totalField, colIndex))
                    {
                        continue;
                    }
                    for (int rowQuadrantStartIndex = 1; rowQuadrantStartIndex <= 7; rowQuadrantStartIndex += 3)
                    {
                        //go through every column in the quadrant
                        for (int rowIndex = rowQuadrantStartIndex; rowIndex < rowQuadrantStartIndex + 3; rowIndex++)
                        {
                            if (progressField[$"row{rowIndex}"][colIndex].Contains(searchNumber.ToString()))
                            {
                                if (rowQuadrantStartIndex == 1)
                                {
                                    countNumberInQuadrant1++;
                                }
                                else if (rowQuadrantStartIndex == 4)
                                {
                                    countNumberInQuadrant2++;
                                }
                                else if (rowQuadrantStartIndex == 7)
                                {
                                    countNumberInQuadrant3++;
                                }
                            }
                        }
                    }
                    if (countNumberInQuadrant1 != 0 && countNumberInQuadrant2 == 0 && countNumberInQuadrant3 == 0)
                    {
                        // anderen beiden Spalten des Quadranten1 bekommen
                        // aus den Reihen die SearchNumber löschen
                        
                        (int col1Index, int col2Index) = GetTwoOtherRowsInQuadrant(colIndex);
                        for (int rowIndex = 1; rowIndex <= 3; rowIndex++)
                        {
                            progressField[$"row{rowIndex}"][col1Index].Remove(searchNumber.ToString());
                            progressField[$"row{rowIndex}"][col2Index].Remove(searchNumber.ToString());
                        }
                    } else if (countNumberInQuadrant1 == 0 && countNumberInQuadrant2 != 0 && countNumberInQuadrant3 == 0)
                    {
                        // anderen beiden Spalten des Quadranten2 bekommen
                        // aus den Reihen die SearchNumber löschen

                        (int col1Index, int col2Index) = GetTwoOtherRowsInQuadrant(colIndex);
                        for (int rowIndex = 4; rowIndex <= 6; rowIndex++)
                        {
                            progressField[$"row{rowIndex}"][col1Index].Remove(searchNumber.ToString());
                            progressField[$"row{rowIndex}"][col2Index].Remove(searchNumber.ToString());
                        }
                    } else if (countNumberInQuadrant1 == 0 && countNumberInQuadrant2 == 0 && countNumberInQuadrant3 != 0)
                    {
                        // anderen beiden Spalten des Quadranten2 bekommen
                        // aus den Reihen die SearchNumber löschen

                        (int col1Index, int col2Index) = GetTwoOtherRowsInQuadrant(colIndex);
                        for (int rowIndex = 7; rowIndex <= 9; rowIndex++)
                        {
                            progressField[$"row{rowIndex}"][col1Index].Remove(searchNumber.ToString());
                            progressField[$"row{rowIndex}"][col2Index].Remove(searchNumber.ToString());
                        }
                    }
                }
            }
            return progressField;
        }

        static Dictionary<string, List<string>[]> CheckPairInRow(Dictionary<string, string[]> totalField, Dictionary<string, List<string>[]> progressField)
        {
            //wenn es in einer Reihe ein Zahlenpaar gibt, in zwei Zellen sind die ausschließlich die gleichen zwei möglichen zahlen
            //können in den anderen Zellen diese Zahl nicht auch sein
            for (int rowIndex = 1; rowIndex <= 9; rowIndex++)
            {
                for (int colIndex = 0; colIndex < 9; colIndex++)
                {
                    List<string> searchPair = new List<string>(progressField[$"row{rowIndex}"][colIndex]);
                    for (int numIndex = 0; numIndex < searchPair.Count; numIndex++)
                    {
                        for (int numIndex2 = 0; numIndex2 < searchPair.Count; numIndex2++)
                        {
                            if (numIndex == numIndex2)
                            {
                                continue;
                            }
                            string num1 = searchPair[numIndex];
                            string num2 = searchPair[numIndex2];
                            int[] pairPlace1 = new int[2];
                            int[] pairPlace2 = new int[2];
                            int countPair = 0;
                            //go through every column in the quadrant
                            for (int colIndex2 = 0; colIndex2 < 9; colIndex2++)
                            {
                                if (progressField[$"row{rowIndex}"][colIndex2].Contains(num1) && progressField[$"row{rowIndex}"][colIndex2].Contains(num2))
                                {
                                    if (countPair == 0)
                                    {
                                        pairPlace1[0] = rowIndex;
                                        pairPlace1[1] = colIndex2;
                                        countPair++;
                                    }
                                    else if (countPair == 1)
                                    {
                                        pairPlace2[0] = rowIndex;
                                        pairPlace2[1] = colIndex2;
                                        countPair++;
                                    } else
                                    {
                                        countPair++;
                                    }
                                }
                                else if ((!progressField[$"row{rowIndex}"][colIndex2].Contains(num1) && progressField[$"row{rowIndex}"][colIndex2].Contains(num2)) || (progressField[$"row{rowIndex}"][colIndex2].Contains(num1) && !progressField[$"row{rowIndex}"][colIndex2].Contains(num2)))
                                {
                                    countPair = 5;
                                }
                            }


                            /*Console.WriteLine($"[{rowIndex}]: {num1}, {num2}; {countPair} -- pairPlace1: [{pairPlace1[0]}, {pairPlace1[1]}], pairPlace2: [{pairPlace2[0]}, {pairPlace2[1]}]");
                            Console.ReadKey();*/
                            if (countPair == 2)
                            {
                                //Console.WriteLine("CountPair = 2");
                                bool numCountInPair1Cell = progressField[$"row{pairPlace1[0]}"][pairPlace1[1]].Count == 2;
                                bool numCountInPair2Cell = progressField[$"row{pairPlace2[0]}"][pairPlace2[1]].Count == 2;
                                for (int columnIndex2 = 0; columnIndex2 < 9; columnIndex2++)
                                {
                                    if ((rowIndex == pairPlace1[0] && columnIndex2 == pairPlace1[1]) || (rowIndex == pairPlace2[0] && columnIndex2 == pairPlace2[1]))
                                    {
                                        progressField[$"row{rowIndex}"][columnIndex2].RemoveAll(item => item != num1 && item != num2);
                                        /*Console.WriteLine("Remove numbers from PairCells");
                                        Console.Write($"[{rowIndex}, {columnIndex2}]: ");
                                        foreach (string num in progressField[$"row{rowIndex}"][columnIndex2])
                                        {
                                            Console.Write(num + ", ");
                                        }
                                        Console.WriteLine();*/
                                    }
                                    else if (numCountInPair1Cell && numCountInPair2Cell)
                                    {
                                        progressField[$"row{rowIndex}"][columnIndex2].Remove(num1);
                                        progressField[$"row{rowIndex}"][columnIndex2].Remove(num2);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return progressField;
        }
        static Dictionary<string, List<string>[]> CheckPairInColumn(Dictionary<string, string[]> totalField, Dictionary<string, List<string>[]> progressField)
        {
            for (int colIndex = 0; colIndex < 9; colIndex++)
            {
                //Console.WriteLine($"colIndex: {colIndex}");
                for (int rowIndex = 1; rowIndex <= 9; rowIndex++)
                {
                    List<string> searchPair = new List<string>(progressField[$"row{rowIndex}"][colIndex]);
                    for (int numIndex = 0; numIndex < searchPair.Count; numIndex++)
                    {
                        for (int numIndex2 = 0; numIndex2 < searchPair.Count; numIndex2++)
                        {
                            if (numIndex == numIndex2)
                            {
                                continue;
                            }
                            string num1 = searchPair[numIndex];
                            string num2 = searchPair[numIndex2];
                            int[] pairPlace1 = new int[2];
                            int[] pairPlace2 = new int[2];
                            int countPair = 0;
                            //go through every column in the quadrant
                            for (int rowIndex2 = 1; rowIndex2 <= 9; rowIndex2++)
                            {
                                if (progressField[$"row{rowIndex2}"][colIndex].Contains(num1) && progressField[$"row{rowIndex2}"][colIndex].Contains(num2))
                                {
                                    if (countPair == 0)
                                    {
                                        pairPlace1[0] = rowIndex2;
                                        pairPlace1[1] = colIndex;
                                        countPair++;
                                    }
                                    else if (countPair == 1)
                                    {
                                        pairPlace2[0] = rowIndex2;
                                        pairPlace2[1] = colIndex;
                                        countPair++;
                                    }
                                    else
                                    {
                                        countPair++;
                                    }
                                }
                                else if ((!progressField[$"row{rowIndex2}"][colIndex].Contains(num1) && progressField[$"row{rowIndex2}"][colIndex].Contains(num2)) || (progressField[$"row{rowIndex2}"][colIndex].Contains(num1) && !progressField[$"row{rowIndex2}"][colIndex].Contains(num2)))
                                {
                                    countPair = 5;
                                }
                            }


                            /*Console.WriteLine($"[{rowIndex}]: {num1}, {num2}; {countPair} -- pairPlace1: [{pairPlace1[0]}, {pairPlace1[1]}], pairPlace2: [{pairPlace2[0]}, {pairPlace2[1]}]");
                            Console.ReadKey();*/
                            if (countPair == 2)
                            {
                                //Console.WriteLine("CountPair = 2");
                                bool numCountInPair1Cell = progressField[$"row{pairPlace1[0]}"][pairPlace1[1]].Count == 2;
                                bool numCountInPair2Cell = progressField[$"row{pairPlace2[0]}"][pairPlace2[1]].Count == 2;
                                for (int rowIndex2 = 1; rowIndex2 <= 9; rowIndex2++)
                                {
                                    if ((rowIndex2 == pairPlace1[0] && colIndex == pairPlace1[1]) || (rowIndex2 == pairPlace2[0] && colIndex == pairPlace2[1]))
                                    {
                                        progressField[$"row{rowIndex2}"][colIndex].RemoveAll(item => item != num1 && item != num2);
                                        /*Console.WriteLine("Remove numbers from PairCells");
                                        Console.Write($"[{rowIndex}, {columnIndex2}]: ");
                                        foreach (string num in progressField[$"row{rowIndex}"][columnIndex2])
                                        {
                                            Console.Write(num + ", ");
                                        }
                                        Console.WriteLine();*/
                                    }
                                    else if (numCountInPair1Cell && numCountInPair2Cell)
                                    {
                                        progressField[$"row{rowIndex2}"][colIndex].Remove(num1);
                                        progressField[$"row{rowIndex2}"][colIndex].Remove(num2);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //Console.ReadKey();
            return progressField;
        }
        static bool TryAnNumber(int countFilledCells, Dictionary<string, string[]> totalField, Dictionary<string, List<string>[]> progressField)
        {
            Console.WriteLine("InTryAnNumber");
            ShowField(totalField);
            Console.ReadKey();
            Dictionary<string, string[]> tryTotalField = totalField.ToDictionary(entry => entry.Key, entry => (string[])entry.Value.Clone());
            //wenn man tryTotalField = totalField mache verweisen beide aufeinander -> wenn man tryTotalField ändert ändert sich auch totalField
            /*wenn man tryTotalField = new Dictionary<string, string[]>(totalField); macht wird zwar eine Kopie des Dictionarys erstellt,
              aber die Arrays in den Dictionarys verweisen immernoch aufeinander*/
            /* Code erklärung:
             *   - .ToDictionary(key, value) erstellt ein neues Dictionary aus totalField
             *   - entry => entry.Key; Key im neuen Dictionary soll der gleiche wie im alten sein
             *   - entry.Value: greift auf den Value des aktuellen Keys zu
             *   - .Clone() erstellt flache Kopie des Arrays; gibt immer ein object zurück
             *   - (string[]) wandelt object in stringArray um
             *   - 
            */
            Dictionary<string, List<string>[]> tryProgressField = progressField
                .ToDictionary(
                    entry => entry.Key,
                    entry => entry.Value.Select(list => new List<string>(list)).ToArray()
                );
            
            int currentCountFilledCells = countFilledCells + 1;
            int lastRoundCountFilledCells = currentCountFilledCells;
            int countRoundsWithoutNumberAdd = 0;
            for (int rowIndex = 1; rowIndex <= 9; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < 9; columnIndex++)
                {
                    if (progressField[$"row{rowIndex}"][columnIndex].Count > 1)
                    {
                        foreach (string number in progressField[$"row{rowIndex}"][columnIndex])
                        {
                            tryTotalField[$"row{rowIndex}"][columnIndex] = number;
                            tryProgressField[$"row{rowIndex}"][columnIndex].Clear();
                            tryProgressField[$"row{rowIndex}"][columnIndex].Add(number);
                            /*ShowField(totalField);
                            ShowField(tryTotalField);
                            Console.ReadKey();*/
                            while (countFilledCells < 81 && countRoundsWithoutNumberAdd < 3)
                            {
                                (bool firstRound, currentCountFilledCells, tryTotalField, tryProgressField) = GetAllPossibleNumbersInSudoku(false, currentCountFilledCells, tryTotalField, tryProgressField);
                                if (countFilledCells == lastRoundCountFilledCells)
                                {
                                    countRoundsWithoutNumberAdd++;
                                } else
                                {
                                    countRoundsWithoutNumberAdd = 0;
                                }
                                lastRoundCountFilledCells = countFilledCells;
                            }
                            //Console.WriteLine("Out of while");
                            
                            //Console.ReadKey();
                            //reset
                            tryTotalField = totalField.ToDictionary(entry => entry.Key, entry => (string[])entry.Value.Clone());
                            tryProgressField = progressField
                .ToDictionary(
                    entry => entry.Key,
                    entry => entry.Value.Select(list => new List<string>(list)).ToArray()
                );
                            /*ShowField(totalField);
                            Console.ReadKey();*/
                            countRoundsWithoutNumberAdd = 0;
                            currentCountFilledCells = countFilledCells + 1;
                            lastRoundCountFilledCells = currentCountFilledCells;
                            if (countFilledCells == 81)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        static (int row1, int row2) GetTwoOtherRowsInQuadrant(int currentRow)
        {
            int row1Index = 0;
            int row2Index = 0;
            switch (currentRow % 3)
            {
                case 0:
                    row1Index = currentRow + 1; //Zeile eins darunter
                    row2Index = currentRow + 2; //Zeile zwei darunter
                    break;
                case 1:
                    row1Index = currentRow - 1; //Zeile drüber
                    row2Index = currentRow + 1; //Zeile drunter
                    break;
                case 2:
                    row1Index = currentRow - 2; //Zeile zwei drüber
                    row2Index = currentRow - 1; //Zeile eins drüber
                    break;
                default: Console.WriteLine("RowQuadrantFailure"); break;
            }
            return (row1Index, row2Index);
        }
    }
}
