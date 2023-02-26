import { GridNode } from "./GridNode"
import type { IGridElement } from "./IGridElement"
import { NeighborDirection } from "./NeighborDirection"

export class Grid {
  readonly nodes: GridNode[][]

  private constructor(nodes: GridNode[][]) {
    this.nodes = nodes
  }

  private AssignNeighbors() {
    for (let i: number = 0; i < this.nodes.length; i++) {
      for (let j: number = 0; j < this.nodes[i].length; j++) {
        if (i > 0) {
          this.nodes[i][j].AssignNeighbor(
            NeighborDirection.Top,
            this.nodes[i - 1][j]
          )
        }
        if (j + 1 < this.nodes[i].length) {
          this.nodes[i][j].AssignNeighbor(
            NeighborDirection.Right,
            this.nodes[i][j + 1]
          )
        } else if (i + 1 < this.nodes.length && j + 1 == this.nodes[i].length) {
          this.nodes[i][j].AssignNeighbor(
            NeighborDirection.Right,
            this.nodes[i + 1][0]
          )
        }
        if (i + 1 < this.nodes.length && j < this.nodes[i + 1].length) {
          this.nodes[i][j].AssignNeighbor(
            NeighborDirection.Bottom,
            this.nodes[i + 1][j]
          )
        }
        if (j > 0) {
          this.nodes[i][j].AssignNeighbor(
            NeighborDirection.Left,
            this.nodes[i][j - 1]
          )
        } else if (i > 0 && j == 0) {
          this.nodes[i][j].AssignNeighbor(
            NeighborDirection.Left,
            this.nodes[i - 1][this.nodes[i - 1].length - 1]
          )
        }
      }
    }
  }

  public static FromGridElements(gridelements: IGridElement[], columnsInRow: number) {
    let currentRowIndex = 0
    let currentRows = [] as GridNode[][]
    let currentColumns = [] as GridNode[]

    const sortedelements = gridelements.sort((a, b) => {
      return a.sort - b.sort
    })

    sortedelements.forEach((element) => {
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
    if (!this.nodes.length) {
      return [] as GridNode[][]
    }

    const columns = [] as GridNode[][]
    let currentColumnRows = [] as GridNode[]
    for (let i: number = 0; i < this.nodes[0].length; i++) {
      for (let j: number = 0; j < this.nodes.length; j++) {
        if (i + 1 <= this.nodes[j].length) {
          currentColumnRows.push(this.nodes[j][i])
        }
        if (j + 1 == this.nodes.length) {
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
    this.nodes.every((row) => {
      node = row.find((node) => elementID == node.element.id)
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
    node.element.sort = neighbor.element.sort
    let currentNode = neighbor
    let currentNeighbor = null as GridNode | null
    while (
      currentNode.HasNeighbor(NeighborDirection.Right) &&
      currentNode.GetNeighbor(NeighborDirection.Right) != node
    ) {
      currentNeighbor = currentNode.GetNeighbor(NeighborDirection.Right)
      currentNode.element.sort = currentNeighbor.element.sort
      currentNode = currentNeighbor
    }
    if (currentNeighbor) {
      ++currentNeighbor.element.sort
    }
  }

  public TryMoveRight(node: GridNode) {
    if (!node.HasNeighbor(NeighborDirection.Right)) {
      return
    }

    const currentsort = node.element.sort
    const neighbor = node.GetNeighbor(NeighborDirection.Right)
    node.element.sort = neighbor.element.sort
    neighbor.element.sort = currentsort
  }

  public TryMoveBottom(node: GridNode) {
    if (!node.HasNeighbor(NeighborDirection.Bottom)) {
      return
    }

    let neighbor = node.GetNeighbor(NeighborDirection.Bottom)
    node.element.sort = neighbor.element.sort
    let currentNode = neighbor
    let currentNeighbor = null as GridNode | null
    while (
      currentNode.HasNeighbor(NeighborDirection.Left) &&
      currentNode.GetNeighbor(NeighborDirection.Left) != node
    ) {
      currentNeighbor = currentNode.GetNeighbor(NeighborDirection.Left)
      currentNode.element.sort = currentNeighbor.element.sort
      currentNode = currentNeighbor
    }
    if (currentNeighbor) {
      --currentNeighbor.element.sort
    }
  }

  public TryMoveLeft(node: GridNode) {
    if (!node.HasNeighbor(NeighborDirection.Left)) {
      return
    }

    const currentsort = node.element.sort
    const neighbor = node.GetNeighbor(NeighborDirection.Left)
    node.element.sort = neighbor.element.sort
    neighbor.element.sort = currentsort
  }
}
