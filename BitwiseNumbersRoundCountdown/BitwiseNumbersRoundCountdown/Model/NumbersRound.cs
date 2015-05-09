using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using DynamicExpression = System.Linq.Dynamic.DynamicExpression;

namespace BitwiseNumbersRoundCountdown.Model
{
    public class NumbersRound
    {
        public List<int> NumbersList { get; }
        public List<string> OperatorsList { get; }
        public int Target { get; }
        public string PlayerSolutionString { get; set; }
        public int PlayerSolution { get; set; }

        public NumbersRound()
        {
            NumbersList = _generateNumbers();
            OperatorsList = new List<string>{ "~", "&", "|", "^", "<<", ">>" };
            Target = _generateTarget();
        }

        private List<int> _generateNumbers()
        {
            List<int> numbers = new List<int>();
            Random random = new Random();
            for (int i = 0; i < 6; i++) numbers.Add(random.Next(0, 100));
            return numbers;
        }

        private int _generateTarget()
        {
            List<int> numbers = new List<int>(NumbersList);
            Random random = new Random();
            int target = _selectNumberFromList(numbers);
            int secondTarget = _selectNumberFromList(numbers);

            while (numbers.Count > 0)
            {
                int randomNum = random.Next(0, 5);
                if (randomNum != 0) secondTarget = _selectNumberFromList(numbers);

                //operator switch
                switch (randomNum)
                {
                    case 0: //NOT
                        target = ~target;
                        break;
                    case 1: //AND
                        target &= secondTarget;
                        break;
                    case 2: //OR
                        target |= secondTarget;
                        break;
                    case 3: //XOR
                        target ^= secondTarget;
                        break;
                    case 4: //LEFTSHIFT
                        target <<= secondTarget;
                        break;
                    case 5: //RIGHTSHIFT
                        target >>= secondTarget;
                        break;
                }
            }

            return target;
        }

        private int _selectNumberFromList(List<int> numbers)
        {
            Random random = new Random();
            int randomNum = random.Next(0, numbers.Count);
            int returnedNum = numbers.Where<int>((n, i) => i == randomNum).First();
            numbers.Remove(returnedNum);
            return returnedNum;
        }

        public bool CheckResult()
        {
            return Target == PlayerSolution;
        }

        public int CalcuateSolution() {
            //doesnt work for bitwise operators
            var lambda = DynamicExpression.ParseLambda(new ParameterExpression[] {}, null, PlayerSolutionString);
            return PlayerSolution = (int) lambda.Compile().DynamicInvoke();
        }

        public int CalcuateSolutionBeta()
        {
            var lambda = DynamicExpression.ParseLambda(new ParameterExpression[] { }, null, PlayerSolutionString);
            return PlayerSolution = (int)lambda.Compile().DynamicInvoke();
        }

    }
}