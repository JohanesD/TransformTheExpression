using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace ONP
{  
    public class Transformator
    {
        private string[] transformedExpressions = null;

        private class Node
        {
            private char operand;
            private Node left;
            private Node right;

            public Node(char operand, Node l, Node r)
            {
                this.operand = operand;
                this.left = l;
                this.right = r;
            }
            // write the expression in postorder
            public void write(StringBuilder sb)
            {
                if (left != null)
                {
                    left.write(sb);
                }
                if (right != null)
                {
                    right.write(sb);
                }
                sb.Append(operand);
            }
        }
        // checking of a Operand
        public static Boolean isOperand(char strToCheck)
        {
            string znak = strToCheck.ToString();
            Regex rg = new Regex(@"[a-z]");
            return rg.IsMatch(znak);
        }

        private const string SYMBOLS_OPERATIONS = "+-*/^";

        private Node getBinaryExpression(string Expression)
        {
            int bracketCounts = 0;
            char Operand;
            int position = -1;
            int operatorIndex = Int32.MaxValue;
            for (int j = 0; j < Expression.Length; j++)
            {
                char znak = Expression[j];
                if (znak.Equals('('))
                {
                    bracketCounts++;
                }
                if (znak.Equals(')'))
                {
                    bracketCounts--;
                }
                int priority = SYMBOLS_OPERATIONS.IndexOf(znak);
                if ((bracketCounts == 0) && (j > 0) && (priority != -1))
                {
                    if (priority < operatorIndex)
                    {
                        operatorIndex = priority;
                        position = j;
                    }
                }
            }
            // expresssion with one operator and two subexpressions
            if (position != -1)
            {
                return new Node(SYMBOLS_OPERATIONS[operatorIndex], getBinaryExpression(Expression.Substring(0, position)), getBinaryExpression(Expression.Substring(position + 1)));
            }
            // expresssion with bracket at the begining and at the end
            if (Expression[0] == '(' && Expression[Expression.Length - 1] == ')')
            {
                return getBinaryExpression(Expression.Substring(1, Expression.Length - 2));
            }
            // expresssion is only one operand
            Operand = Char.Parse(Expression);
            try
            {
                if (isOperand(Operand))
                {
                    return new Node(Operand, null, null);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Problem with operand" + e.Message);
            }
        }

        Transformator.Node root;
        public Transformator(string[] arrayExpressions)
        {
            transformedExpressions = new string[arrayExpressions.Length];
            StringBuilder sb = null;
            for (int i = 0; i < arrayExpressions.Length; i++)
            {
                string Expression = arrayExpressions[i];
                root = getBinaryExpression(Expression);
                sb = new StringBuilder();
                root.write(sb);
                string postOrder = sb.ToString();
                transformedExpressions[i] = postOrder;
            }
        }
        public string[] getPostOrders()
        {
            return transformedExpressions;
        }
    }
    class TransformTheExpression
    {
        static void Main(string[] args)
        {
            string[] expressions = null;

            string numberString = Console.ReadLine();
            int numberExpressions;
            if (Int32.TryParse(numberString, out numberExpressions))
            {
                if (numberExpressions > 0 && numberExpressions <= 100)
                {
                    expressions = new string[numberExpressions];

                    for (int i = 0; i < numberExpressions; i++)
                    {
                        string expression = Console.ReadLine();
                        if (expression.Length <= 400)
                        {
                            expressions[i] = expression;
                        }
                    }
                    Transformator p = new Transformator(expressions);
                    string[] tranformedExpressions = p.getPostOrders();
                    for (int i = 0; i < tranformedExpressions.Length; i++)
                    {
                        Console.WriteLine(tranformedExpressions[i]);
                    }
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }
}


