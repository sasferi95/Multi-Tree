using System;
using System.Collections.Generic;
using System.Linq;

namespace Multi_tree
{
    class TreeManager
    {
        List<Element> elements;
        Element root=null;

        public Element Root { 
            get 
            {
                if (root == null)
                    throw new NullReferenceException();
                return root; 
            } }

        public TreeManager(List<Element> elements)
        {
            this.elements = elements;
        }

        public void BuildTree()
        {
            root = BuildTreeFromElements();
        }

        Element BuildTreeFromElements()
        {
            Element startingElement = elements.Where(x => x.ParentId == null).FirstOrDefault();
            startingElement.Children = GetChildrenOfElement(startingElement.ElementId);
            BuildChildren(startingElement.Children);
            return startingElement;
        }

        void BuildChildren(List<Element> children)
        {
            foreach (var child in children)
            {
                child.Children = GetChildrenOfElement(child.ElementId);
                if (child.Children.Count != 0)
                    BuildChildren(child.Children);
            }
        }

        List<Element> GetChildrenOfElement(int id)
        {
            return elements.Where(x => x.ParentId == id).ToList();
        }

        public void DisplayTree()
        {
            DisplayTree(root);
        }

        void DisplayTree(Element actualElement, int depth = 0)
        {
            if(actualElement.SelectionState == SelectionStates.Selected)
                Console.ForegroundColor = ConsoleColor.Green;
            else if(actualElement.SelectionState==SelectionStates.RerecursivelySelected)
                Console.ForegroundColor = ConsoleColor.Red;
            else
                Console.ForegroundColor = ConsoleColor.White;

            string offset = new string('\t', depth);
            Console.WriteLine(offset+actualElement.ElementId.ToString()+" "+actualElement.Content);

            foreach(var element in actualElement.Children)
            {
                if(actualElement.ExpansionState==ExpansionStates.Expanded)
                    DisplayTree(element, depth + 1);
            }
        }

        public void ChangeTreeElementExpandStatus(int id)
        {
            var element= RecursiveTreeElementSearch(root, id);
            if (element.ExpansionState == ExpansionStates.Expanded)
                element.ExpansionState = ExpansionStates.Collapsed;
            else
                element.ExpansionState = ExpansionStates.Expanded;
        }

        Element RecursiveTreeElementSearch(Element element, int id)
        {
            foreach(Element currelement in element.Children)
            {
                if (element.ElementId == id)
                    return element;
                else
                    return RecursiveTreeElementSearch(currelement, id);
            }
            throw new Exception("A keresett elem nem található");
        }

        public void ChangeTreeElementSelection(int id)
        {
            var element = RecursiveTreeElementSearch(root, id);
            if (element.SelectionState == SelectionStates.Selected || element.SelectionState == SelectionStates.RerecursivelySelected)
                UnSelect(element);
            else
                Select(element);
        }

        void Select(Element element)
        {
            if (element.ExpansionState == ExpansionStates.Expanded)
                element.SelectionState = SelectionStates.Selected;
            else if (element.ExpansionState == ExpansionStates.Collapsed)
                DfsStatusChangeForSubtree(element, SelectionStates.RerecursivelySelected);
        }

        void UnSelect(Element element)
        {
            if (element.ExpansionState == ExpansionStates.Expanded)
                element.SelectionState = SelectionStates.NotSelected;
            else if (element.ExpansionState == ExpansionStates.Collapsed)
                DfsStatusChangeForSubtree(element, SelectionStates.NotSelected);
        }

        void DfsStatusChangeForSubtree(Element subtreeRoot, SelectionStates newSelectionState)
        {
            foreach (var child in subtreeRoot.Children)
            {
                DfsStatusChangeForSubtree(child, newSelectionState);
            }
            subtreeRoot.SelectionState = newSelectionState;
        }

    }


}
