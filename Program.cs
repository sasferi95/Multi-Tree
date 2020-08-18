using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Deployment.Internal;
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
            }; 
            
            
            TreeManager treeManager=new TreeManager(elements);
            treeManager.BuildTree();
            Menu(treeManager);
            Console.ReadLine();
        }

        static void Menu(TreeManager treeManager)
        {
            bool exit = false;

            while (!exit)
            { 
                Console.Clear();
                treeManager.DisplayTree();
                
                string command = Console.ReadLine();

                string[] commandPieces = command.Split(' ');
                if (CheckInput(commandPieces))
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

                        case "exit":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid command");
                            break;
                    }
                }
                else
                    Console.WriteLine("Invalid command");
            }
        }

        static bool CheckInput(string[] command)
        {
            return (command[0] == "cs" || command[0] == "ce") && int.TryParse(command[1],out int b) || command[0] == "exit" ;
        }
    }


}
