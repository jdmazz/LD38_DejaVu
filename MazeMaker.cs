/*
Maze generator using stack, DFS, rng adjacent directions, and prefab instantion.
Non-trivial class in and of itself and I made it during a 48 game jam. Booyah!
BTW, this maze generator is based on Jarník's algorithm. He's a very smart guy, as
most mathematicians are. Read about him on wiki:
https://en.wikipedia.org/wiki/Vojt%C4%9Bch_Jarn%C3%ADk
2017/4/23
@author jdmazz
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

class Pair {
	public Pair(int x, int y) {
		this.x = x;
		this.y = y;
	}

	public int x, y;
}

enum Dir {LEFT,UP,RIGHT,DOWN, MARK};

public class MazeMaker : MonoBehaviour {
	public int rows = 3;
	public int cols = 3;
	public Transform floor;
	public Transform[] walls;
	public Transform[] witches;
	public Transform[] barriers;

	void Awake ()
	{
		int[,,] maze = new int[rows, cols, 5]; // Start with a grid full of walls.
		int[,] stage = new int[3 * rows, 3 * cols];
		Pair p = new Pair (0, 0);
		Stack<Pair> st = new Stack<Pair> ();
		st.Push (new Pair(p.x,p.y));
		while (st.Count > 0) {
			maze [p.x, p.y, 4] = 1; // Pick a cell, mark it as part of the maze.
			List<Dir> dirs = new List<Dir> (); // Add the walls of the cell to the wall list.
			if (p.y > 0 && maze [p.x, p.y - 1, 4] == 0) 
				dirs.Add (Dir.LEFT);
			if (p.x > 0 && maze[p.x-1,p.y,4] == 0)
				dirs.Add(Dir.UP);
			if (p.y < cols-1 && maze[p.x,p.y+1,4] == 0)
				dirs.Add(Dir.RIGHT);
			if (p.x < rows-1 && maze[p.x+1,p.y,4] == 0)
				dirs.Add(Dir.DOWN);

			if (dirs.Count > 0) { // While there are walls in the list: 
				st.Push(new Pair(p.x,p.y));
				Dir rngDir = dirs [Random.Range (0, dirs.Count)]; // Pick a random wall from the list.
				switch (rngDir) {
				case Dir.LEFT:
					maze[p.x,p.y--,0] = 1; // Make the wall a passage 
					maze[p.x,p.y,2] = 1; // and mark the unvisited cell as part of the maze.
				break;
				case Dir.UP:
					maze[p.x--,p.y,1] = 1;
					maze[p.x,p.y,3] = 1;
				break;
				case Dir.RIGHT:
					maze[p.x,p.y++,2] = 1;
					maze[p.x,p.y,0] = 1;
				break;
				case Dir.DOWN:
					maze[p.x++,p.y,3] = 1;
					maze[p.x,p.y,1] = 1;
				break;
				}
			} else {
				p = st.Pop(); // Remove the wall from the list.
			}
		}
		RenderMaze(maze, stage);
		InstantiateWitches(maze);
		GameObject.Find ("GameManager").GetComponent<GameManager> ().makingStage = false;
	}

	void ShowStack(Stack<Pair> st) {
		StringBuilder sb = new StringBuilder();
		foreach (Pair s in st)
			sb.Append("("+s.x+","+s.y+")");
		print(sb.ToString());
	}

	// Calculates where to actually render the maze
	void RenderMaze (int[,,] maze, int[,] stage)
	{
		for (int m = 0; m < rows; ++m) {
			for (int n = 0; n < cols; ++n) {
				List<int> treeData = new List<int> ();
				for (int i = 0; i < 5; ++i)
					treeData.Add(maze [m, n, i]);
				for (int i = 3*m+1; i < 3*m+2; ++i) {
					for (int j = 3*n+1; j < 3*n+2; ++j)
		        		stage[i, j] = 1;

					if (treeData[0] == 1) {
						for (int j = 3*m+1; j < 3*m+2; ++j)
							stage[j,3*n] = 1;
					}
					if (treeData[1] == 1) {
						for (int j = 3*n+1; j < 3*n+2; ++j)
							stage[3*m,j] = 1;
					}
					if (treeData[2] == 1) {
						for (int j = 3*m+1; j < 3*m+2; ++j)
							stage[j,3*n+2] = 1;
					}
					if (treeData[3] == 1) {
						for (int j = 3*n+1; j < 3*n+2; ++j)
							stage[3*m+2,j] = 1;
					}
				}
			}
		}
		RenderStage(stage);
	}

	// Renders the small world
	void RenderStage(int[,] stage) {
		for (int i = 0; i < 3*rows; ++i) {
			for (int j = 0; j < 3*cols; ++j) {
				Transform floorTrans = Instantiate(floor.transform);
				floorTrans.SetParent(transform);
				floorTrans.localPosition = new Vector3(j,-i,0);
				if (stage[i,j] == 0) {
					Transform wallTransU = Instantiate(walls[Random.Range (0, walls.Length)]);
					wallTransU.SetParent(transform);
					wallTransU.localPosition = new Vector3(j,-i,0);
				}
			}
		}
		// render outer barriers
		for (int j = 0; j <3*cols; ++j) {
			Transform barTrans1 = Instantiate(barriers[0]);
			barTrans1.SetParent(transform);
			barTrans1.localPosition = new Vector3(j,1,0);
			Transform barTrans2 = Instantiate(barriers[0]);
			barTrans2.SetParent(transform);
			barTrans2.localPosition = new Vector3(j,-3*rows,0);
			barTrans2.localRotation = Quaternion.Euler(0,0,180);
		}
		for (int i = 0; i <3*rows; ++i) {
			Transform barTrans1 = Instantiate(barriers[0]);
			barTrans1.SetParent(transform);
			barTrans1.localPosition = new Vector3(-1,-i,0);
			barTrans1.localRotation = Quaternion.Euler(0,0,90);
			Transform barTrans2 = Instantiate(barriers[0]);
			barTrans2.SetParent(transform);
			barTrans2.localPosition = new Vector3(3*cols,-i,0);
			barTrans2.localRotation = Quaternion.Euler(0,0,270);
		}
		Transform ul = Instantiate(barriers[1]);
		ul.SetParent(transform);
		ul.localPosition = new Vector3(-1,1,0);
		ul.localRotation = Quaternion.Euler(0,0,90);
		Transform ur = Instantiate(barriers[1]);
		ur.SetParent(transform);
		ur.localPosition = new Vector3(3*cols,1,0);
		Transform ll = Instantiate(barriers[1]);
		ll.SetParent(transform);
		ll.localPosition = new Vector3(-1,-3*rows,0);
		ll.localRotation = Quaternion.Euler(0,0,180);
		Transform lr = Instantiate(barriers[1]);
		lr.SetParent(transform);
		lr.localPosition = new Vector3(3*cols,-3*rows,0);
		lr.localRotation = Quaternion.Euler(0,0,270);
	}

	void InstantiateWitches (int[,,] maze)
	{
		for (int i = 1; i < rows; ++i) {
			for (int j = 1; j < cols; ++j) {
				if (Random.Range (0f, 1f) > .1) {
					Transform witchTrans = Instantiate (witches [Random.Range (0, witches.Length)]);
					witchTrans.SetParent (transform);
					witchTrans.localPosition = new Vector3 (3*j+1, -3*i-1, 0);
				}
			}
		}
	}
}