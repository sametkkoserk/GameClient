using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using Runtime.Contexts.Lobby.Vo;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Contexts.Lobby.View.JoinLobbyPanel
{
  public class JoinLobbyPanelView : EventView, IEnhancedScrollerDelegate
  {
    public GameObject lobbyListLoadingIcon;

    public List<LobbyVo> lobbies=new List<LobbyVo>(); 
    public EnhancedScroller scroller;
    public EnhancedScrollerCellView cellViewPrefab;
    public float cellSize ;

    protected override void Start()
    {
      base.Start();
      scroller.Delegate = this;
    }
    public void OnBack()
    {
      dispatcher.Dispatch(JoinLobbyPanelEvent.Back);
    }

    public void OnRefreshList()
    {
      dispatcher.Dispatch(JoinLobbyPanelEvent.RefreshList);
    }
    
    #region EnhancedScroller Handlers

    /// <summary>
    /// This tells the scroller the number of cells that should have room allocated. This should be the length of your data array.
    /// </summary>
    /// <param name="scroller">The scroller that is requesting the data size</param>
    /// <returns>The number of cells</returns>
    public int GetNumberOfCells(EnhancedScroller scroller)
    {
      // in this example, we just pass the number of our data elements
      return lobbies.Count;
    }

    /// <summary>
    /// This tells the scroller what the size of a given cell will be. Cells can be any size and do not have
    /// to be uniform. For vertical scrollers the cell size will be the height. For horizontal scrollers the
    /// cell size will be the width.
    /// </summary>
    /// <param name="scroller">The scroller requesting the cell size</param>
    /// <param name="dataIndex">The index of the data that the scroller is requesting</param>
    /// <returns>The size of the cell</returns>
    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
      // in this example, even numbered cells are 30 pixels tall, odd numbered cells are 100 pixels tall
      return cellSize;
    }

    /// <summary>
    /// Gets the cell to be displayed. You can have numerous cell types, allowing variety in your list.
    /// Some examples of this would be headers, footers, and other grouping cells.
    /// </summary>
    /// <param name="scroller">The scroller requesting the cell</param>
    /// <param name="dataIndex">The index of the data that the scroller is requesting</param>
    /// <param name="cellIndex">The index of the list. This will likely be different from the dataIndex if the scroller is looping</param>
    /// <returns>The cell for the scroller to use</returns>
    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
      // first, we get a cell from the scroller by passing a prefab.
      // if the scroller finds one it can recycle it will do so, otherwise
      // it will create a new cell.
      JoinLobbyPanelItemBehaviour cellView = scroller.GetCellView(cellViewPrefab) as JoinLobbyPanelItemBehaviour;

      // set the name of the game object to the cell's data index.
      // this is optional, but it helps up debug the objects in 
      // the scene hierarchy.
      cellView.name = lobbies[dataIndex].lobbyName;

      // in this example, we just pass the data to our cell's view which will update its UI
      cellView.SetData(lobbies[dataIndex], () =>
      {
        dispatcher.Dispatch(JoinLobbyPanelEvent.Join, lobbies[dataIndex].lobbyCode);
      });

      // return the cell to the scroller
      return cellView;
    }

    #endregion
  }
}