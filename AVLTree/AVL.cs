using System;
using System.ComponentModel.Design.Serialization;

public class AVL<T> where T : IComparable<T>
{
	private Node<T> root;

	public Node<T> Root
	{
		get => this.root;
	}

	public void Insert(T item)
	{
		this.root = this.Insert(this.root, item);
	}

	private Node<T> Insert(Node<T> node, T item)
	{
		if (node == null)
		{
			return new Node<T>(item);
		}

		int cmp = item.CompareTo(node.Value);
		if (cmp < 0)
		{
			node.Left = this.Insert(node.Left, item);
		}
		else if (cmp > 0)
		{
			node.Right = this.Insert(node.Right, item);
		}

		UpdateHeight(node);
		node = Balance(node);

		return node;
	}

	public static void UpdateHeight(Node<T> node)
	{
		node.Height = Math.Max(Height(node.Left), Height(node.Right)) + 1;
	}

	public static int Height(Node<T> node)
	{
		if (node == null)
			return 0;

		return node.Height;
	}

	private static Node<T> Balance(Node<T> node)
	{
		int balance = Height(node.Left) - Height(node.Right);

		// 1. if tree is right-heavy
		if (balance < -1)
		{
			balance = Height(node.Right.Left) - Height(node.Right.Right);

			// 1.1. if right.right is heavier than right.left => do right rotation
			if (balance <= 0)
				return RotateLeft(node);

			// 1.2. else if right.left is heavier than right.right => do right-left rotation
			node.Right = RotateRight(node.Right);

			return RotateLeft(node);
		}

		// 2. else if tree is left-heavy
		if (balance > 1)
		{
			balance = Height(node.Left.Left) - Height(node.Left.Right);

			// 2.1. if left.left is heavier than left.right => do right rotation
			if (balance > 0)
				return RotateRight(node);

			// 2.2. else if left.right is heavier than left.left => do left-right rotation
			node.Left = RotateLeft(node.Left);
			return RotateRight(node);
		}

		return node;
	}

	public bool Contains(T item)
	{
		var node = this.Search(this.root, item);
		return node != null;
	}

	public void EachInOrder(Action<T> action)
	{
		this.EachInOrder(this.root, action);
	}

	private Node<T> Search(Node<T> node, T item)
	{
		if (node == null)
			return null;

		int cmp = item.CompareTo(node.Value);

		if (cmp < 0)
			return Search(node.Left, item);

		if (cmp > 0)
			return Search(node.Right, item);

		return node;
	}

	private void EachInOrder(Node<T> node, Action<T> action)
	{
		if (node == null)
			return;

		this.EachInOrder(node.Left, action);
		action(node.Value);
		this.EachInOrder(node.Right, action);
	}

	private static Node<T> RotateLeft(Node<T> node)
	{
		var right = node.Right;
		node.Right = right.Left;
		right.Left = node;

		UpdateHeight(node);
		UpdateHeight(right);

		return right;
	}

	private static Node<T> RotateRight(Node<T> node)
	{
		var left = node.Left;
		node.Left = left.Right;
		left.Right = node;

		UpdateHeight(node);
		UpdateHeight(left);

		return left;
	}
}
