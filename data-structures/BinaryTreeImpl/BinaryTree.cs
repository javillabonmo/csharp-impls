namespace BinaryTreeImpl
{
    internal class BinarySearchTree<T> where T : IComparable<T>
    {
        public TreeNode<T>? Root { get; private set; }

        public void Insert(T value)
        {
            // Insercion con balanceo rojo-negro para mantener el arbol en O(log n).
            InsertRedBlack(Root, value);
        }
        public void Delete(T value)
        {
            Root = DeleteRecursively(Root, value);
        }

        public List<T> TraverseInOrder()
        {
            List<T> result = new List<T>();
            return InorderTraversal(result, Root);
        }




        public bool Search(T value)
        {
            return SearchRecursively(Root, value);
        }


        //O(log(n))
        private TreeNode<T> InsertRecursively(TreeNode<T>? current, T value)
        {

            // If we hit a null spot, insert the new node here
            if (current == null) return new TreeNode<T>(value);


            // Compare values to determine direction
            // what if the value is equal to the current node's value? In this implementation, we will not insert duplicates.
            int comparison = value.CompareTo(current.Value);

            if (comparison < 0)
            {
                current.Left = InsertRecursively(current.Left, value);
            }
            else if (comparison > 0)
            {
                current.Right = InsertRecursively(current.Right, value);
            }

            return current;

        }


        private void InsertRedBlack(TreeNode<T>? current, T value)
        {
            TreeNode<T> newNode = new TreeNode<T>(value) { Color = NodeColor.Red };
            TreeNode<T>? parent = null;



            while (current != null)
            {
                parent = current;

                int comparison = newNode.Value.CompareTo(current.Value);

                if (comparison < 0)
                {
                    current = current.Left;
                }
                else if (comparison > 0)
                {
                    current = current.Right;
                }
                else
                {
                    // No se insertan duplicados.
                    return;
                }
            }

            newNode.Parent = parent;

            if (parent == null)
            {
                Root = newNode;
            }
            else if (newNode.Value.CompareTo(parent.Value) < 0)
            {
                parent.Left = newNode;
            }
            else
            {
                parent.Right = newNode;
            }


            FixInsert(newNode);
        }


        private void FixInsert(TreeNode<T> newNode)
        {
            while (newNode != Root && newNode.Parent!.Color == NodeColor.Red)
            {
                if (newNode.Parent == newNode.Parent.Parent!.Left)
                {
                    TreeNode<T>? uncle = newNode.Parent.Parent.Right;

                    if (uncle != null && uncle.Color == NodeColor.Red)
                    {
                        newNode.Parent.Color = NodeColor.Black;
                        uncle.Color = NodeColor.Black;
                        newNode.Parent.Parent.Color = NodeColor.Red;
                        newNode = newNode.Parent.Parent;
                    }
                    else
                    {
                        if (newNode == newNode.Parent.Right)
                        {
                            newNode = newNode.Parent;
                            RotateLeft(newNode);
                        }

                        newNode.Parent!.Color = NodeColor.Black;
                        newNode.Parent!.Parent!.Color = NodeColor.Red;
                        RotateRight(newNode.Parent!.Parent!);
                    }
                }
                else
                {
                    TreeNode<T>? uncle = newNode.Parent.Parent.Left;

                    if (uncle != null && uncle.Color == NodeColor.Red)
                    {
                        newNode.Parent.Color = NodeColor.Black;
                        uncle.Color = NodeColor.Black;
                        newNode.Parent.Parent.Color = NodeColor.Red;
                        newNode = newNode.Parent.Parent;
                    }
                    else
                    {
                        if (newNode == newNode.Parent.Left)
                        {
                            newNode = newNode.Parent;
                            RotateRight(newNode);
                        }

                        newNode.Parent!.Color = NodeColor.Black;
                        newNode.Parent!.Parent!.Color = NodeColor.Red;
                        RotateLeft(newNode.Parent!.Parent!);
                    }
                }
            }

            Root!.Color = NodeColor.Black;
        }

        private void FixDelete()
        {
            //fix de eliminacion con balanceo rojo-negro
        }

        private void RotateLeft(TreeNode<T> x)
        {
            TreeNode<T> y = x.Right!;

            x.Right = y.Left;
            if (y.Left != null)
            {
                y.Left.Parent = x;
            }

            y.Parent = x.Parent;
            if (x.Parent == null)
            {
                Root = y;
            }
            else if (x == x.Parent.Left)
            {
                x.Parent.Left = y;
            }
            else
            {
                x.Parent.Right = y;
            }

            y.Left = x;
            x.Parent = y;
        }

        private void RotateRight(TreeNode<T> x)
        {
            TreeNode<T> y = x.Left!;

            x.Left = y.Right;
            if (y.Right != null)
            {
                y.Right.Parent = x;
            }

            y.Parent = x.Parent;
            if (x.Parent == null)
            {
                Root = y;
            }
            else if (x == x.Parent.Right)
            {
                x.Parent.Right = y;
            }
            else
            {
                x.Parent.Left = y;
            }

            y.Right = x;
            x.Parent = y;
        }


        //(O(log(n)))
        private TreeNode<T> getMinimum(TreeNode<T> node)
        {
            while (node.Left != null)
            {
                node = node.Left;
            }
            return node;
        }

        //(O(log(n)))
        private TreeNode<T> getMaximum(TreeNode<T> node)
        {
            while (node.Right != null)
            {
                node = node.Right;
            }
            return node;
        }


        //O(log(n))
        private TreeNode<T>? DeleteRecursively(TreeNode<T>? currentNode, T value)
        {
            if (currentNode == null)
            {
                return null;
            }

            int comparison = value.CompareTo(currentNode.Value);

            if (comparison < 0)
            {
                currentNode.Left = DeleteRecursively(currentNode.Left, value);
            }
            else if (comparison > 0)
            {
                currentNode.Right = DeleteRecursively(currentNode.Right, value);
            }
            else
            {
                // Node with only one child or no child
                if (currentNode.Left == null)
                {
                    return currentNode.Right;
                }
                else if (currentNode.Right == null)
                {
                    return currentNode.Left;
                }

                // Node with two children: Get the inorder successor (smallest in the right subtree)
                TreeNode<T> temp = getMinimum(currentNode.Right);

                // Copy the inorder successor's content to this node
                currentNode.Value = temp.Value;

                // Delete the inorder successor
                currentNode.Right = DeleteRecursively(currentNode.Right, temp.Value);
            }

            return currentNode;
        }


        private void DeleteRedBlack(TreeNode<T> node)
        {
            // Implementación de eliminación con balanceo rojo-negro.
        }
        private List<T> PreorderTraversal(List<T> result, TreeNode<T>? node)
        {
            if (node == null)
            {
                return result;
            }

            result.Add(node.Value);
            PreorderTraversal(result, node.Left);
            PreorderTraversal(result, node.Right);

            return result;
        }

        private List<T> PostorderTraversal(List<T> result, TreeNode<T>? node)
        {
            if (node == null)
            {
                return result;
            }

            PostorderTraversal(result, node.Left);
            PostorderTraversal(result, node.Right);
            result.Add(node.Value);

            return result;
        }

        private List<T> InorderTraversal(List<T> result, TreeNode<T>? node)
        {
            if (node == null)
            {
                return result;
            }

            InorderTraversal(result, node.Left);
            result.Add(node.Value);
            InorderTraversal(result, node.Right);

            return result;
        }


        private bool SearchRecursively(TreeNode<T>? currentNode, T value)
        {
            if (currentNode == null) return false;


            int comparison = value.CompareTo(currentNode.Value);

            if (comparison == 0) return true; // Value found

            return comparison < 0
            ? SearchRecursively(currentNode.Left, value)
            : SearchRecursively(currentNode.Right, value);
        }

        private int Height(TreeNode<T>? node)
        {
            if (node == null) return 0;


            int leftHeight = Height(node.Left);
            int rightHeight = Height(node.Right);

            return Math.Max(leftHeight, rightHeight) + 1;
        }


    }
}
