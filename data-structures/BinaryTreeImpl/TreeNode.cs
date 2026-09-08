namespace BinaryTreeImpl
{

    public class TreeNode<T>// red-black tree node implementation
    {
        public T Value { get; set; }
        public TreeNode<T>? Left { get; set; }
        public TreeNode<T>? Right { get; set; }

        public NodeColor Color { get; set; } = NodeColor.Black;
        public TreeNode<T>? Parent { get; set; }

        public TreeNode(T value)
        {
            Value = value;
        }
    }

    public enum NodeColor
    {
        Red,
        Black
    }
}