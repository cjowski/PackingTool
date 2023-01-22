import type { IGridElement } from "./IGridElement"
import type { NeighborDirection } from "./NeighborDirection"

export class GridNode {
  Element: IGridElement
  private _neighbors: Map<NeighborDirection, GridNode>

  private constructor(
    element: IGridElement,
    neighbors: Map<NeighborDirection, GridNode>
  ) {
    this.Element = element
    this._neighbors = neighbors
  }

  public static NoNeighbors(element: IGridElement) {
    return new GridNode(element, new Map<NeighborDirection, GridNode>())
  }

  public AssignNeighbor(direction: NeighborDirection, neighbor: GridNode) {
    this._neighbors.set(direction, neighbor)
  }

  public HasNeighbor(direction: NeighborDirection) {
    return this._neighbors.has(direction)
  }

  public GetNeighbor(direction: NeighborDirection) {
    return this._neighbors.get(direction)!
  }
}
