using System;

namespace MasterMind
{
    internal class Program
    {
        private const int MaxNumber = 6666;
        private const int MinNumber = 1111;
        private const int MaxIndividualNumber = 6;
        private const int MinIndividualNumber = 1;
        static void Main(string[] args)
        {
            //Loop necessary to play multiple games
            while (true)
            {
                //Intro Sequence & Game Setup
                Console.WriteLine("Welcome to Mastermind!");
                System.Threading.Thread.Sleep(2000);

                var intGuesses = 10;
                var intSecretCode = GenerateSecretCode();

                bool winState = false;

                bool[] guessAry = { false, false, false, false };
                bool[] answerAry = { false, false, false, false };
                Console.Clear();

                //Guesses Loop
                while (intGuesses > 0)
                {
                    Console.WriteLine("Guesses Remaining: " + intGuesses.ToString());

                    Console.WriteLine("\nMake your guess:\n");
                    string strUserGuess = Console.ReadLine();


                    if (IsGuessCorrectFormat(ref strUserGuess))
                    {
                        var intUserGuess = int.Parse(strUserGuess);

                        if (intUserGuess == intSecretCode) //Game has been won.
                        {
                            winState = true;
                            break;
                        }

                        var inPlaceCount = GetInPlaceCount(intUserGuess, guessAry, answerAry, intSecretCode);
                        var outOfPlaceCount = GetOutOfPlaceCount(intUserGuess, guessAry, answerAry, intSecretCode);

                        string strFeedback = "\nScore: ";

                        //Switch statement builds feedback string.
                        #region Switch statement w/cases
                        switch (inPlaceCount)
                        {
                            case 0:
                                break;
                            case 1:
                                strFeedback += "+";
                                break;
                            case 2:
                                strFeedback += "++";
                                break;
                            case 3:
                                strFeedback += "+++";
                                break;
                        }
                        switch (outOfPlaceCount)
                        {
                            case 0:
                                break;
                            case 1:
                                strFeedback += "-";
                                break;
                            case 2:
                                strFeedback += "--";
                                break;
                            case 3:
                                strFeedback += "---";
                                break;
                            case 4:
                                strFeedback += "----";
                                break;
                        }
                        #endregion

                        Console.WriteLine(strFeedback + "\n");
                        Console.WriteLine("--------------------\n");
                        intGuesses--;
                    }
                    else
                        Console.WriteLine("Make sure your input is between 1111 and 6666, with each digit being no larger that 6.");
                }
                if (winState)
                {
                    Console.WriteLine("--------------------\n");
                    Console.WriteLine("\nYou solved it!");
                }
                else
                {
                    Console.WriteLine("\nYou lose. :(\n");
                    Console.WriteLine("The code was " + intSecretCode);
                }
                if (EndGameDisplay())
                {
                    Console.Clear();
                    continue;
                }
                break;
            }
        }

        #region Functions

        /// <summary>
        /// Displays the End Game screen.
        /// </summary>
        /// <returns>True if the user wishes to play again, false otherwise</returns>
        private static bool EndGameDisplay()
        {
            Console.WriteLine("\nWould you like to play again? (Y/N)\n");
            while (true)
            {
                string strPlayAgain = Console.ReadLine();
                if (strPlayAgain == "N" || strPlayAgain == "n" || strPlayAgain == "No" || strPlayAgain == "no")
                {
                    return false;
                }
                else if (strPlayAgain == "Y" || strPlayAgain == "y" || strPlayAgain == "Yes" || strPlayAgain == "yes")
                {
                    return true;
                }
                Console.WriteLine("\nPlease enter either a Y or a N.\n");
            }
        }

        /// <summary>
        /// Calculates the number of digits that are correct and in place.
        /// </summary>
        /// <param name="intUserGuess">The input provided by the user entered guess</param>
        /// <param name="guessAry">The array that determines if the user guess digits are in the correct place.</param>
        /// <param name="answerAry"></param>
        /// <param name="intSecretCode">The secret code.</param>
        /// <returns>The number of in place digits.</returns>
        private static int GetInPlaceCount(int intUserGuess, bool[] guessAry, bool[] answerAry, int intSecretCode)
        {
            for (var i = 0; i < 4; i++)
            {
                guessAry[i] = false;
                answerAry[i] = false;
            }

            var inPlaceCount = 0;
            var tempGuess = intUserGuess;
            var tempRand = intSecretCode;

            for (var i = 0; i < 4; i++)
            {
                var guessDigit = tempGuess % 10;
                tempGuess = tempGuess / 10;
                var randDigit = tempRand % 10;
                tempRand = tempRand / 10;

                if (guessDigit == randDigit)
                {
                    guessAry[i] = true;
                    answerAry[i] = true;
                    inPlaceCount++;
                }
            }
            return inPlaceCount;
        }

        /// <summary>
        /// Calulates the number of digits that are correct, but out of place
        /// </summary>
        /// <param name="intUserGuess">The input provided by the user entered guess</param>
        /// <param name="guessAry">The array that determines if the user guess digits are in the correct place.</param>
        /// <param name="answerAry"></param>
        /// <param name="intSecretCode">The secret code.</param>
        /// <returns>The number of out of place digits</returns>
        public static int GetOutOfPlaceCount(int intUserGuess, bool[] guessAry, bool[] answerAry, int intSecretCode)
        {
            int outOfPlaceCount = 0;

            for (int i = 0; i < 4; i++)
            {
                var guessDigit = intUserGuess % 10;
                intUserGuess = intUserGuess / 10;
                int tempRand = intSecretCode;
                if (guessAry[i] == false)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        var randDigit = tempRand % 10;
                        tempRand = tempRand / 10;
                        if (answerAry[j] == false)
                        {
                            if (guessDigit == randDigit)
                            {
                                outOfPlaceCount++;
                                guessAry[i] = true;
                                answerAry[j] = true;
                                break;
                            }
                        }
                    }
                }
            }
            return outOfPlaceCount;
        }

        /// <summary>
        /// Checks to see if the user guess is correct.
        /// </summary>
        /// <param name="strUserGuess">The string representation of the user entered guess, passed by reference so that the string is updated in Main.</param>
        /// <returns>True, if guess is correct, False otherwise</returns>
        private static bool IsGuessCorrectFormat(ref string strUserGuess)
        {
            try
            {
                var intUserGuess = int.Parse(strUserGuess);
                var tempGuess = intUserGuess;
                for (int i = 0; i < 4; i++)
                {
                    var guessDigit = tempGuess % 10;
                    if (guessDigit > MaxIndividualNumber || guessDigit < MinIndividualNumber || intUserGuess < MinNumber || intUserGuess > MaxNumber)
                        throw (new Exception());
                    tempGuess = tempGuess / 10;
                }
            }
            catch
            {
                Console.WriteLine("\nMake sure your input is between 1111 and 6666, with each digit being no larger that 6.");
                System.Threading.Thread.Sleep(2000);
                Console.WriteLine("\nMake your guess:\n");
                strUserGuess = Console.ReadLine();
                if (IsGuessCorrectFormat(ref strUserGuess))
                    return true;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Generates a code from 1111 to 6666
        /// </summary>
        /// <returns>An int from 1111 to 6666</returns>
        private static int GenerateSecretCode()
        {
            string strCraftedNum = "";
            Random srand = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < 4; i++)
            {
                strCraftedNum = strCraftedNum.Insert(strCraftedNum.Length, srand.Next(MinIndividualNumber, MaxIndividualNumber).ToString());
            }
            return int.Parse(strCraftedNum);
        }

        #endregion
    }
}
