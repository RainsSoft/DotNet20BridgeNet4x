using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet20BridgeNet4x.Test
{
    class task_test9
    {
        //动态并行(TaskCreationOptions.AttachedToParent) 父任务等待所有子任务完成后 整个任务才算完成（示例）：
        class Node
        {
            public Node Left { get; set; }
            public Node Right { get; set; }
            public string Text { get; set; }
        }
        static Node GetNode() {
            Node root = new Node {
                Left = new Node() {
                    Left = new Node {
                        Text = "L-L"
                    },
                    Right = new Node {
                        Text = "L-R"
                    },
                    Text = "L"
                },
                Right = new Node() {
                    Left = new Node {
                        Text = "R-L"
                    },
                    Right = new Node {
                        Text = "R-R"
                    },
                    Text = "R"
                },
                Text = "Root"
            };
            return root;
        }
        static void DisplayTree(Node root) {
            var task = Task.Factory.StartNew(() => {
                DisplayNode(root);
            }, CancellationToken.None,
            TaskCreationOptions.None,
            TaskScheduler.Default);
            task.Wait();
        }
        static void DisplayNode(Node current) {
            if (current.Left != null) {
                Task.Factory.StartNew(() => DisplayNode(current.Left),
                    CancellationToken.None,
                    TaskCreationOptions.AttachedToParent,
                    TaskScheduler.Default);
            }
            if (current.Right != null) {
                Task.Factory.StartNew(() => DisplayNode(current.Right),
                    CancellationToken.None,
                    TaskCreationOptions.AttachedToParent,
                    TaskScheduler.Default);
            }
            Console.WriteLine("当前节点值为：" + current.Text + "  处理ThreadID=" + Thread.CurrentThread.ManagedThreadId);
        }
        public static void test() {
            Node root = GetNode();
            DisplayTree(root);
        }
    }
}
