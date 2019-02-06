using System;

namespace AVLTree
{
    public class AVLTree<T>
    {
        private class Node
        {
            internal int key;
            internal int height;
            internal T value;
            internal Node left;
            internal Node right;
            internal Node parent;
            internal Node(int key, int height, Node left, Node right, Node parent, T value)
            {
                this.key = key;
                this.height = height;
                this.value = value;
                this.left = left;
                this.right = right;
                this.parent = parent;
            }
        }
        public int Count { get; private set; } // количество элементов дерева
        private Node root; // корень
        public AVLTree()
        {
            Count = 0;
        }

        public bool Add(int key, T value) // возрващает истину, если вставка прошла успешно и ключ уникальный
        {
            if (Count == 0)
            {
                root = new Node(key, 1, null, null, null, value);
            }
            else
            {
                Node iter = root;
                Node current = null;
                while (current == null)
                {
                    if (key < iter.key)
                    {
                        if (iter.left != null)
                            iter = iter.left;
                        else
                        {
                            current = new Node(key, 1, null, null, iter, value);
                            iter.left = current;
                        }
                    }
                    else if (key > iter.key)
                    {
                        if (iter.right != null)
                            iter = iter.right;
                        else
                        {
                            current = new Node(key, 1, null, null, iter, value);
                            iter.right = current;
                        }
                    }
                    else
                        return false;

                }
                Balance(current.parent);
            }
            Count++;
            return true;
        }

        private void Balance(Node current)
        {
            if (Math.Abs(IfNullHeight(current.left) - IfNullHeight(current.right)) > 1)
                MakeTurn(current); // повороты

            if (ReDefineHeight(current) && current.parent != null) // перерасчет высоты узла
                Balance(current.parent);
        }

        private bool ReDefineHeight(Node current) // возвращает ложь, если высота текущего узла не была изменена
        {
            if (IfNullHeight(current.left) < IfNullHeight(current.right) &&
                IfNullHeight(current.right) + 1 != current.height)
            {
                current.height = current.right.height + 1;
                return true;
            }

            else if (IfNullHeight(current.left) > IfNullHeight(current.right) &&
                IfNullHeight(current.left) + 1 != current.height)
            {
                current.height = current.left.height + 1;
                return true;
            }

            else if (current.left != null &&
                IfNullHeight(current.left) == IfNullHeight(current.right) &&
                IfNullHeight(current.left) + 1 != current.height)
            {
                current.height = current.left.height + 1;
                return true;
            }

            else if (current.left == null && current.right == null)
            {
                current.height = 1;
                return true;
            }

            return false;
        }

        private int IfNullHeight(Node current)
        {
            return current == null ? 0 : current.height;
        }

        private void MakeTurn(Node current)
        {
            if (IfNullHeight(current.right) > IfNullHeight(current.left) &&
                IfNullHeight(current.right.left) <= IfNullHeight(current.right.right))
                SmallLeftTurn(current);

            else if (IfNullHeight(current.right) > IfNullHeight(current.left) &&
                IfNullHeight(current.right.left) > IfNullHeight(current.right.right))
                BigLeftTurn(current);

            else if (IfNullHeight(current.right) < IfNullHeight(current.left) &&
                IfNullHeight(current.left.right) <= IfNullHeight(current.left.left))
                SmallRightTurn(current);

            else if (IfNullHeight(current.right) < IfNullHeight(current.left) &&
                IfNullHeight(current.left.right) > IfNullHeight(current.left.left))
                BigRightTurn(current);
        }

        private void SmallLeftTurn(Node current)
        {
            Node tempRight = current.right;
            Node temp = current;
            Node tempParrent = current.parent;
            current.parent = current.right;
            if (current.right.left != null)
            {
                current.right.left.parent = current;
                current.right = current.right.left;
            }
            else
                current.right = null;
            current = tempRight;
            current.left = temp;
            if (current.right != null)
                current.right.parent = current;

            if (tempParrent != null)
            {
                current.parent = tempParrent;
                if (current.key < current.parent.key)
                    current.parent.left = current;
                else
                    current.parent.right = current;
            }
            else
            {
                current.parent = null;
                root = current;
            }
            ReDefineHeight(current.left);
            ReDefineHeight(current);
        }

        private void BigLeftTurn(Node current)
        {
            SmallRightTurn(current.right);
            SmallLeftTurn(current);
        }
        private void SmallRightTurn(Node current)
        {
            Node tempLeft = current.left;
            Node tempLeftRight = current.left.right;
            Node temp = current;
            Node tempParrent = current.parent;
            current.parent = current.left;
            if (current.left.right != null)
            {
                current.left.right.parent = current;
                current.left = current.left.right;
            }
            else
                current.left = null;
            current = tempLeft;
            current.right = temp;
            current.right.left = tempLeftRight;

            if (tempParrent != null)
            {
                current.parent = tempParrent;
                if (current.key < current.parent.key)
                    current.parent.left = current;
                else
                    current.parent.right = current;
            }
            else
            {
                current.parent = null;
                root = current;
            }
            ReDefineHeight(current.right);
            ReDefineHeight(current);
        }

        private void BigRightTurn(Node current)
        {
            SmallLeftTurn(current.left);
            SmallRightTurn(current);
        }

        private Node FindNode(int key)
        {
            Node current = root;
            for (int i = 0; i < root.height; i++)
            {
                if (current == null)
                    break;

                else if (key < current.key)
                    current = current.left;
                else if (key > current.key)
                    current = current.right;
                else
                    return current;
            }
            return null;
        }

        private Node FindForReplace(Node fromNode)
        {
            Node current = fromNode.left;
            while (current != null)
            {
                if (current.right != null)
                    current = current.right;
                else
                    return current;
            }
            return null;
        }

        public bool Erase(int key) // возвращает истину, если ключ найден и удаление прошло успешно
        {
            Node forDelete = FindNode(key);

            if (forDelete == null)
                return false;

            Node forReplace = FindForReplace(forDelete);
            Node balanceFromNode = null; // узел, с которого начинается балансировка

            if (forReplace != null)
            {
                Node temp = forReplace;

                if (forDelete != forReplace.parent)
                    balanceFromNode = forReplace.parent;
                else
                    balanceFromNode = forReplace;

                Node tempDel = forDelete;
                Node tempDelParent = forDelete.parent;
                if (forReplace.left != null)
                {
                    if (forReplace.parent != forDelete)
                    {
                        forReplace.left.parent = forReplace.parent;

                        if (forReplace.key < forReplace.parent.key)
                            forReplace.parent.left = forReplace.left;
                        else
                            forReplace.parent.right = forReplace.left;
                    }
                }

                if (forReplace.parent.right == forReplace)
                    forReplace.parent.right = null;


                temp.right = forDelete.right;
                if (forDelete.right != null)
                    forDelete.right.parent = temp;

                temp.parent = forDelete.parent;
                if (forDelete.parent != null)
                {
                    if (forReplace.key < forDelete.parent.key)
                        forDelete.parent.left = temp;
                    else
                        forDelete.parent.right = temp;
                }

                if (forDelete.left != forReplace)
                {
                    temp.left = forDelete.left;
                    forDelete.left.parent = temp;
                }

                forDelete = temp;
                if (temp.parent == null)
                    root = temp;

            }
            else
            {
                if (forDelete.right != null)
                {
                    if (forDelete.parent != null)
                    {
                        if (forDelete.key < forDelete.parent.key)
                            forDelete.parent.left = forDelete.right;

                        else
                            forDelete.parent.right = forDelete.right;

                        forDelete.right.parent = forDelete.parent;
                    }
                    else
                    {
                        forDelete.right.parent = null;
                        root = forDelete.right;
                    }

                    forDelete = forDelete.right;
                    balanceFromNode = forDelete;
                }
                else
                {
                    if (forDelete.parent != null)
                    {
                        if (forDelete.key < forDelete.parent.key)
                            forDelete.parent.left = null;

                        else
                            forDelete.parent.right = null;
                        balanceFromNode = forDelete.parent;
                    }
                    else
                        root = null;
                }
            }

            Action<Node> ReBalance = null;
            ReBalance = (Node current) =>
            {
                if (Math.Abs(IfNullHeight(current.left) - IfNullHeight(current.right)) > 1)
                    MakeTurn(current); // повороты

                if ((ReDefineHeight(current) || current != forDelete.parent)
                && current.parent != null) // перерасчет высоты узла
                    ReBalance(current.parent);
            };

            if (balanceFromNode != null)
                ReBalance.Invoke(balanceFromNode);

            Count--;
            return true;
        }

        public T At(int key)
        {
            Node current = root;
            for (int i = 0; i < root.height; i++)
            {
                if (current == null)
                    break;

                else if (key < current.key)
                    current = current.left;
                else if (key > current.key)
                    current = current.right;
                else
                    return current.value;
            }
            return default(T);
        }

        public bool Exist(int key)
        {
            Node current = root;
            for (int i = 0; i < root.height; i++)
            {
                if (current == null)
                    break;

                else if (key < current.key)
                    current = current.left;
                else if (key > current.key)
                    current = current.right;
                else
                    return true;
            }
            return false;
        }
        public void Print()
        {
            Node current = root;
            Action print = () => Console.Write(current.key.ToString() + ' ');
            EnumOperation(ref current, print);
        }

        public int[] ToArray()
        {
            Node current = root;
            int[] arr = new int[Count];
            int i = 0;
            Action toArray = () =>
            {
                arr[i] = current.key;
                i++;
            };
            EnumOperation(ref current, toArray);
            return arr;
        }

        private void EnumOperation(ref Node current, Action operation) // перебор дерева с действием operation
        {
            if (current.left != null)
            {
                current = current.left;
                EnumOperation(ref current, operation);
                current = current.parent;
            }

            operation.Invoke();

            if (current.right != null)
            {
                current = current.right;
                EnumOperation(ref current, operation);
                current = current.parent;
            }
        }

        private Node[] GetItemsFromLine(Node[] fromElems) // возвращает правые и левые ветки
        {
            Node[] res = new Node[fromElems.Length * 2];
            for (int i = 0, k = 0; i < res.Length; i++, k++)
            {
                res[i] = fromElems[k]?.left;
                res[++i] = fromElems[k]?.right;
            }
            return res;
        }

        public void Draw()
        {
            if (root == null)
                return;

            int line = 50;
            Node[] arr = { root };
            int[] arrXCoord = { line };
            int[] arrNewXCoord = null;
            Console.SetCursorPosition(arrXCoord[0], 0);
            Console.Write(arr[0].key);
            for (int i = 1; i < root.height; i++)
            {
                arr = GetItemsFromLine(arr);
                arrNewXCoord = new int[arr.Length];
                for (int a = 0, newxi = 0, xi = 0; a < arr.Length; a++, xi++, newxi++)
                {
                    if (arr[a] == null && arr[a + 1] == null)
                    {
                        a++;
                        newxi++;
                        continue;
                    }
                    Console.SetCursorPosition(arrXCoord[xi] - line / 2, i);
                    for (int symb = 0; symb < line; symb++)
                        Console.Write('-');

                    // left value
                    int newLeftValueXCoord = arrXCoord[xi] - line / 2;
                    Console.SetCursorPosition(newLeftValueXCoord, i);
                    Console.Write(arr[a] == null ? 0 + "" : arr[a].key.ToString());
                    arrNewXCoord[newxi] = newLeftValueXCoord;

                    // right value 
                    int rightValueLength = arr[++a] == null ? 1 : arr[a].key.ToString().Length;
                    int newRightValueXCoord = arrXCoord[xi] +
                        Convert.ToInt32(Math.Round(1.0 * line / 2, MidpointRounding.AwayFromZero)) - rightValueLength;
                    Console.SetCursorPosition(newRightValueXCoord, i);
                    Console.Write(arr[a] == null ? 0 + "" : arr[a].key.ToString());
                    arrNewXCoord[++newxi] = newRightValueXCoord;
                }

                arrXCoord = arrNewXCoord;
                line /= 2;
            }
        }
    }
}
