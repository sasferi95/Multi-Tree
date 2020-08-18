using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multi_tree
{
    public enum ExpansionStates { Expanded, Collapsed }
    public enum SelectionStates { NotSelected, Selected, RerecursivelySelected }

    /// <summary>
    /// Represents an element in the tree
    /// </summary>
    public class Element
    {
        public int ElementId { get; set; }
        public int? ParentId { get; set; }

        public ExpansionStates ExpansionState { get; set; }
        public SelectionStates SelectionState { get; set; }

        public List<Element> Children { get; set; }
        string content;
        public string Content 
        { 
            get { return content; }
        }

        public Element(int id, int? parentId, string content)
        {
            this.ElementId = id;
            this.ParentId = parentId;
            this.content = content;

            this.ExpansionState = ExpansionStates.Expanded;
            this.SelectionState = SelectionStates.NotSelected;
            this.Children = new List<Element> { };
        }
    }
}
