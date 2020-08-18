using System;
using System.Collections.Generic;
using System.Linq;

namespace Multi_tree
{
    /// <summary>
    /// Responsible class to manage the tree
    /// </summary>
    class TreeManager
    {
        List<Element> elements;
        Element root=null;

        //public Element Root { 
        //    get 
        //    {
        //        if (root == null)
        //            throw new NullReferenceException();
        //        return root; 
        //    } }

        public TreeManager(List<Element> elements)
        {
            this.elements = elements;
        }

        /// <summary>
        /// public method to build the multi tree from the tree elemenets
        /// </summary>
        public void BuildTree()
        {
            if (elements != null)
                root = BuildTreeFromElements();
        }

        /// <summary>
        /// gets the first element which does not have a parent and builds the tree from that element
        /// </summary>
        /// <returns>the root element if exists</returns>
        Element BuildTreeFromElements()
        {
            Element startingElement = elements.Where(x => x.ParentId == null).FirstOrDefault();
            if (startingElement != null)
            {
                startingElement.Children = GetChildrenOfElement(startingElement.ElementId);
                BuildChildren(startingElement.Children);
            }
            return startingElement;
        }
        /// <summary>
        /// recursively builds the tree
        /// </summary>
        /// <param name="children">the list of children of given element</param>
        void BuildChildren(List<Element> children)
        {
            foreach (var child in children)
            {
                child.Children = GetChildrenOfElement(child.ElementId);
                if (child.Children.Count != 0)
                    BuildChildren(child.Children);
            }
        }

        /// <summary>
        /// returns the children elements of a element with a given id
        /// </summary>
        /// <param name="id">parent element's id</param>
        /// <returns>list of children</returns>
        List<Element> GetChildrenOfElement(int id)
        {
            return elements.Where(x => x.ParentId == id).ToList();
        }

        /// <summary>
        /// Public method that allows to display the tree
        /// without parameter because the root element does not have to be public
        /// </summary>
        public void DisplayTree()
        {
            DisplayTreeElement(root);
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// displays the element with corresponding color to its selectionstate
        /// green means selected
        /// red means rerecursively selected
        /// white means the element is not selected
        /// </summary>
        /// <param name="actualElement">the element to display</param>
        /// <param name="depth">the number of tabs to make the display meaningful</param>
        void DisplayTreeElement(Element actualElement, int depth = 0)
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
                    DisplayTreeElement(element, depth + 1);
            }
        }

        /// <summary>
        /// change the expand status of an element with given id
        /// </summary>
        /// <param name="id">id of element that expand status has to be changed</param>
        public void ChangeTreeElementExpandStatus(int id)
        {
            Element element = default(Element);
            RecursiveTreeElementSearch(root, id, ref element);

            if (element == default(Element))
            {
                throw new Exception("A keresett elem nem található");
            }

            if (element.ExpansionState == ExpansionStates.Expanded)
                element.ExpansionState = ExpansionStates.Collapsed;
            else
                element.ExpansionState = ExpansionStates.Expanded;
        }

        /// <summary>
        /// to be able to choose an element in the tree we have to find it based on it's id
        /// resursively search through the tree
        /// </summary>
        /// <param name="element">the tested element</param>
        /// <param name="id">searched element's id </param>
        /// <param name="found">if we find the element with the id this parameter will be responsible to contain that element</param>
        void RecursiveTreeElementSearch(Element element, int id, ref Element found)
        {
            if (element.ElementId == id)
            {
                found = element;
                return;
            }
            if (element.ExpansionState == ExpansionStates.Expanded)
            {
                foreach (Element currelement in element.Children)
                {
                    if (found != null)
                        return;
                    if (currelement.ElementId == id)
                    {
                        found = currelement;
                        return;
                    }
                    else
                        RecursiveTreeElementSearch(currelement, id, ref found);
                }
            }
            
        }

        /// <summary>
        /// change the select status of an element with given id
        /// </summary>
        /// <param name="id">id of element that Selection status has to be changed according to the described logic</param>
        public void ChangeTreeElementSelection(int id)
        {
            Element element = null;
            RecursiveTreeElementSearch(root, id, ref element);

            if (element == null)
            {
                throw new Exception("A keresett elem nem található");
            }

            if (element.SelectionState == SelectionStates.Selected || element.SelectionState == SelectionStates.RerecursivelySelected)
                UnSelect(element);
            else
                Select(element);
        }

        /// <summary>
        /// Select the element and if necessary all of it's children
        /// </summary>
        /// <param name="element">the element that has been selected</param>
        void Select(Element element)
        {
            if (element.ExpansionState == ExpansionStates.Expanded)
                element.SelectionState = SelectionStates.Selected;
            else if (element.ExpansionState == ExpansionStates.Collapsed)
                DfsStatusChangeForSubtree(element, SelectionStates.RerecursivelySelected);
        }

        /// <summary>
        /// Unelect the element and if necessary all of it's children
        /// </summary>
        /// <param name="element">>the element that has been unselected</param>
        void UnSelect(Element element)
        {
            if (element.ExpansionState == ExpansionStates.Expanded)
                element.SelectionState = SelectionStates.NotSelected;
            else if (element.ExpansionState == ExpansionStates.Collapsed)
                DfsStatusChangeForSubtree(element, SelectionStates.NotSelected);
        }

        /// <summary>
        /// recursive method to change the status of the child elements and the root's
        /// </summary>
        /// <param name="subtreeRoot">starting element</param>
        /// <param name="newSelectionState">changed status</param>
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
