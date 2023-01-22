import { GridNode } from "./GridNode"
import type { IGridElement } from "./IGridElement"
import { NeighborDirection } from "./NeighborDirection"

export class Grid {
  readonly Nodes: GridNode[][]

  private constructor(nodes: GridNode[][]) {
    this.Nodes = nodes
  }

  private AssignNeighbors() {
    for (let i: number = 0; i < this.Nodes.length; i++) {
      for (let j: number = 0; j < this.Nodes[i].length; j++) {
        if (i > 0) {
          this.Nodes[i][j].AssignNeighbor(
            NeighborDirection.Top,
            this.Nodes[i - 1][j]
          )
        }
        if (j + 1 < this.Nodes[i].length) {
          this.Nodes[i][j].AssignNeighbor(
            NeighborDirection.Right,
            this.Nodes[i][j + 1]
          )
        } else if (i + 1 < this.Nodes.length && j + 1 == this.Nodes[i].length) {
          this.Nodes[i][j].AssignNeighbor(
            NeighborDirection.Right,
            this.Nodes[i + 1][0]
          )
        }
        if (i + 1 < this.Nodes.length && j < this.Nodes[i + 1].length) {
          this.Nodes[i][j].AssignNeighbor(
            NeighborDirection.Bottom,
            this.Nodes[i + 1][j]
          )
        }
        if (j > 0) {
          this.Nodes[i][j].AssignNeighbor(
            NeighborDirection.Left,
            this.Nodes[i][j - 1]
          )
        } else if (i > 0 && j == 0) {
          this.Nodes[i][j].AssignNeighbor(
            NeighborDirection.Left,
            this.Nodes[i - 1][this.Nodes[i - 1].length - 1]
          )
        }
      }
    }
  }

  public static FromGridElements(gridElements: IGridElement[], columnsInRow: number) {
    let currentRowIndex = 0
    let currentRows = [] as GridNode[][]
    let currentColumns = [] as GridNode[]

    const sortedElements = gridElements.sort((a, b) => {
      return a.Sort - b.Sort
    })

    sortedElements.forEach((element) => {
      if (currentColumns.length < columnsInRow) {
        currentColumns.push(GridNode.NoNeighbors(element))
      }
      if (currentColumns.length == columnsInRow) {
        currentRows[currentRowIndex] = currentColumns
        currentColumns = [] as GridNode[]
        ++currentRowIndex
      }
    })

    if (currentColumns.length && currentColumns.length < columnsInRow) {
      currentRows[currentRowIndex] = currentColumns
    }

    const grid = new Grid(currentRows)
    grid.AssignNeighbors()
    return grid
  }

  public GetColumns() {
    if (!this.Nodes.length) {
      return [] as GridNode[][]
    }

    const columns = [] as GridNode[][]
    let currentColumnRows = [] as GridNode[]
    for (let i: number = 0; i < this.Nodes[0].length; i++) {
      for (let j: number = 0; j < this.Nodes.length; j++) {
        if (i + 1 <= this.Nodes[j].length) {
          currentColumnRows.push(this.Nodes[j][i])
        }
        if (j + 1 == this.Nodes.length) {
          columns.push(currentColumnRows)
          currentColumnRows = [] as GridNode[]
          break
        }
      }
    }
    return columns
  }

  public GetNode(elementID: number) {
    let node = undefined as GridNode | undefined
    this.Nodes.every((row) => {
      node = row.find((node) => elementID == node.Element.ID)
      if (node !== undefined) {
        return false
      }
      return true
    })
    return node!
  }

  public TryMoveTop(node: GridNode) {
    if (!node.HasNeighbor(NeighborDirection.Top)) {
      return
    }

    let neighbor = node.GetNeighbor(NeighborDirection.Top)
    node.Element.Sort = neighbor.Element.Sort
    let currentNode = neighbor
    let currentNeighbor = null as GridNode | null
    while (
      currentNode.HasNeighbor(NeighborDirection.Right) &&
      currentNode.GetNeighbor(NeighborDirection.Right) != node
    ) {
      currentNeighbor = currentNode.GetNeighbor(NeighborDirection.Right)
      currentNode.Element.Sort = currentNeighbor.Element.Sort
      currentNode = currentNeighbor
    }
    if (currentNeighbor) {
      ++currentNeighbor.Element.Sort
    }
  }

  public TryMoveRight(node: GridNode) {
    if (!node.HasNeighbor(NeighborDirection.Right)) {
      return
    }

    const currentSort = node.Element.Sort
    const neighbor = node.GetNeighbor(NeighborDirection.Right)
    node.Element.Sort = neighbor.Element.Sort
    neighbor.Element.Sort = currentSort
  }

  public TryMoveBottom(node: GridNode) {
    if (!node.HasNeighbor(NeighborDirection.Bottom)) {
      return
    }

    let neighbor = node.GetNeighbor(NeighborDirection.Bottom)
    node.Element.Sort = neighbor.Element.Sort
    let currentNode = neighbor
    let currentNeighbor = null as GridNode | null
    while (
      currentNode.HasNeighbor(NeighborDirection.Left) &&
      currentNode.GetNeighbor(NeighborDirection.Left) != node
    ) {
      currentNeighbor = currentNode.GetNeighbor(NeighborDirection.Left)
      currentNode.Element.Sort = currentNeighbor.Element.Sort
      currentNode = currentNeighbor
    }
    if (currentNeighbor) {
      --currentNeighbor.Element.Sort
    }
  }

  public TryMoveLeft(node: GridNode) {
    if (!node.HasNeighbor(NeighborDirection.Left)) {
      return
    }

    const currentSort = node.Element.Sort
    const neighbor = node.GetNeighbor(NeighborDirection.Left)
    node.Element.Sort = neighbor.Element.Sort
    neighbor.Element.Sort = currentSort
  }
}
