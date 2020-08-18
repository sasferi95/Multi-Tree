using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Deployment.Internal;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Multi_tree
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Element> elements = new List<Element>
            {
                new Element(0, null,"(G)Root"),
                new Element(1, 0, "Tony Stark"),
                new Element(2, 5, "Steve Rogers"),
                new Element(3, 1, "Bruce Banner"),
                new Element(4, 2, "Thor"),
                new Element(5, 3, "Natasha Romanoff"),
                new Element(6, 3, "Clint Barton"),
                new Element(7, 3, "James Rhodes"),
                new Element(8, 1, "Scott Lang"),
                new Element(9, 9, "Doctor Strange"),
                new Element(10, 0, "Peter Parker"),
            }; 
            
            
            TreeManager treeManager=new TreeManager(elements);
            treeManager.BuildTree();
            Menu(treeManager);
        }

        /// <summary>
        /// displays the tree and wait for commands
        /// </summary>
        /// <param name="treeManager">Tree manager that contains the tree that will be displayed and allows the tree operations</param>
        static void Menu(TreeManager treeManager)
        {
            bool exit = false;
            string line=new string('-', 20);
            while (!exit)
            {
                Console.Clear();
                treeManager.DisplayTree();
                Console.WriteLine();
                Console.WriteLine(line);
                Console.WriteLine("To select item use \"cs x\" command. Where x is the id of the Element.");
                Console.WriteLine("To expand/collapse item use \"ce x\" command. Where x is the id of the Element.");
                Console.WriteLine("Only visible elements properties can be changed.");
                Console.WriteLine(line);
                string command = Console.ReadLine();

                if (command == "exit")
                    exit = true;
                else
                {
                    string[] commandPieces = command.Split(' ');
                    if (CheckInput(commandPieces))
                    {
                        try
                        {
                            int treeElementId = int.Parse(commandPieces[1]);
                            switch (commandPieces[0])
                            {
                                case "cs":
                                    treeManager.ChangeTreeElementSelection(treeElementId);
                                    break;

                                case "ce":
                                    treeManager.ChangeTreeElementExpandStatus(treeElementId);
                                    break;
                            }
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine(e.Message);
                            Console.ReadLine();
                        }
                    }
                    else
                        Console.WriteLine("Invalid command");
                }
                
            }
        }

        /// <summary>
        /// checks if the given command is valid or not 
        /// </summary>
        /// <param name="command">[0] command, [1] element id</param>
        /// <returns>the command is valid or invalid</returns>
        static bool CheckInput(string[] command)
        {
            return (command[0] == "cs" || command[0] == "ce") && int.TryParse(command[1],out int b);
        }
    }


}
