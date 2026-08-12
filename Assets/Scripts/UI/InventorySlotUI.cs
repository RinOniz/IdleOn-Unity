using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public int slotIndex;

    private PlayerInventory playerInventory;
    private RectTransform iconRect;
    private CanvasGroup iconCanvasGroup;
    private Vector2 originalAnchoredPos;

    private Transform originalParent;
    private Canvas rootCanvas;

    private void Start()
    {
        // tu dong nhan dien vi tri cua slot trong danh sach
        slotIndex = transform.GetSiblingIndex();

        // tu dong tim PlayerInventory trong scene
        playerInventory = FindFirstObjectByType<PlayerInventory>();

        // tim anh icon trong slot
        iconRect = transform.Find("Item_Icon").GetComponent<RectTransform>();

        // lap them CanvasGroup de dieu khien raycast cua icon
        iconCanvasGroup = iconRect.gameObject.AddComponent<CanvasGroup>();

        rootCanvas = GetComponentInParent<Canvas>();
    }

    // bat dau keo: luu vi tri goc cua icon va tat raycast cua icon
    public void OnBeginDrag(PointerEventData eventData)
    {
        //originalIconPos = iconRect.position; // luu vi tri goc
        //iconCanvasGroup.blocksRaycasts = false; // tat can chuot cua icon de co the keo ra ngoai

        originalAnchoredPos = iconRect.anchoredPosition;

        originalParent = iconRect.parent; 

        iconRect.SetParent(rootCanvas.transform);
        iconRect.SetAsLastSibling(); 

        iconCanvasGroup.blocksRaycasts = false;
    }

    // trong luc keo: di chuyen icon theo con tro chuot
    public void OnDrag(PointerEventData eventData)
    {
        if (playerInventory.slots[slotIndex].amount == 0)
        {
            return;
        }

        // FIX "TÚM TÓC": Di chuyển cực kỳ mượt mà bất chấp độ phân giải màn hình
        iconRect.anchoredPosition += eventData.delta / rootCanvas.scaleFactor;

        //iconRect.position = Input.mousePosition; // bat icon di chuyen theo con tro chuot
    }

    // tha chuot: tra icon ve vi tri cu va bat lai raycast cua icon, kiem tra xem co tha vao slot khac hay khong
    public void OnEndDrag(PointerEventData eventData)
    {
        // Trả Icon về lại nhà cũ trong lưới
        iconRect.SetParent(originalParent);
        iconRect.anchoredPosition = originalAnchoredPos;
        iconCanvasGroup.blocksRaycasts = true;

        // VẤN ĐỀ 4: Quét xem chuột đang nằm ở đâu
        GameObject hitObject = eventData.pointerCurrentRaycast.gameObject;

        // Nếu thả ra ngoài không khí (null) hoặc thả vào nơi KHÔNG PHẢI là ô đồ
        if (hitObject == null || hitObject.GetComponentInParent<InventorySlotUI>() == null)
        {
            playerInventory.DropItem(slotIndex); // Quăng đồ!
        }

        //// tra icon ve vi tri cu va bat lai raycast cua icon
        //iconRect.position = originalIconPos;
        //iconCanvasGroup.blocksRaycasts = true;

        //// check vut do ra ngoai: lay doi tuong ma con tro chuot dang o tren no
        //GameObject hitObject = eventData.pointerEnter;

        //// neu chuot roi ra ngoai bang, hoac roi vao cho khong phai o do
        //if (hitObject == null || hitObject.GetComponentInParent<InventorySlotUI>() == null)
        //{
        //    playerInventory.DropItem(slotIndex); 
        //}
    }

    // khi co mot icon khac duoc tha vao slot nay: trao doi du lieu giua 2 slot
    public void OnDrop(PointerEventData eventData)
    {
        // lay thong tin cua slot dang duoc keo
        InventorySlotUI draggedSlot = eventData.pointerDrag.GetComponent<InventorySlotUI>();

        // neu slot dang duoc keo khac slot hien tai, thi trao doi du lieu giua 2 slot
        if (draggedSlot != null && draggedSlot != this)
        {
            playerInventory.SwapSlots(draggedSlot.slotIndex, this.slotIndex);
        }
    }
}
