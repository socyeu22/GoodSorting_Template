using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using HighlightPlus;

/// <summary>
/// Quản lý thông tin và thao tác của một món hàng trên kệ.
/// </summary>
public class WaresController : MonoBehaviour
{
    /// <summary>
    /// Tham chiếu tới kệ chứa món hàng hiện tại.
    /// </summary>
    public ShelfController shelfController;

    //public HighlightEffect highlightEffect;

    /// <summary>
    /// Định danh cho loại món hàng.
    /// </summary>
    public int waresID;

    /// <summary>
    /// Chỉ số cột của món hàng trong kệ.
    /// </summary>
    public int colID;

    /// <summary>
    /// Chỉ số độ sâu (hàng) của món hàng trong kệ.
    /// </summary>
    public int deptID;

    /// <summary>
    /// Thuộc tính chỉ đọc phản hồi vị trí cột hiện tại.
    /// </summary>
    public int ColID
    {
        get
        {
            return colID;
        }

        private set
        {
            colID = value;
        }
    }

    /// <summary>
    /// Thuộc tính chỉ đọc phản hồi vị trí hàng sâu hiện tại.
    /// </summary>
    public int DeptID
    {
        get
        {
            return deptID;
        }

        private set
        {
            deptID = value;
        }
    }

    /// <summary>
    /// Danh sách vật liệu để thay đổi màu sắc cho món hàng.
    /// </summary>
    public Material[] material;

    /// <summary>
    /// Renderer dùng để bật/tắt hiển thị món hàng.
    /// </summary>
    public MeshRenderer meshRenderer;

    /// <summary>
    /// Transform của mô hình hiển thị món hàng.
    /// </summary>
    private Transform modelTrans;

    /// <summary>
    /// Lưu vị trí cũ của món hàng khi bị di chuyển.
    /// </summary>
    private int oldIndex;

    /// <summary>
    /// Các trạng thái xử lý của món hàng.
    /// </summary>
    public enum STATE
    {
        IDLE,
        PROCESS
    }

    /// <summary>
    /// Trạng thái hiện tại của món hàng.
    /// </summary>
    public STATE currentState;

    /// <summary>
    /// Hàm khởi tạo mặc định của Unity.
    /// </summary>
    void Start()
    {

    }

    /// <summary>
    /// Hàm cập nhật mỗi khung hình của Unity.
    /// </summary>
    void Update()
    {

    }

    /// <summary>
    /// Khởi tạo dữ liệu renderer và trạng thái ban đầu của món hàng.
    /// </summary>
    public void InitWares()
    {
        if (transform.childCount > 0)
        {
            modelTrans = transform.GetChild(0);
            material = modelTrans.GetComponent<MeshRenderer>().materials;
            meshRenderer = modelTrans.GetComponent<MeshRenderer>();
            //modelTrans.gameObject.AddComponent<HighlightEffect>();
            //highlightEffect = modelTrans.GetComponent<HighlightEffect>();
        }
        else
        {
            material = GetComponent<MeshRenderer>().materials;
            meshRenderer = GetComponent<MeshRenderer>();
            gameObject.AddComponent<HighlightEffect>();
            //highlightEffect = GetComponent<HighlightEffect>();
        }

        //highlightEffect.outlineColor = Color.red;
        //highlightEffect.outlineWidth = 0.15f;
        //highlightEffect.highlighted = false;
        currentState = STATE.IDLE;
    }

    /// <summary>
    /// Đổi màu vật liệu thành xám để thể hiện trạng thái ẩn.
    /// </summary>
    public void SetColorHide()
    {
        //material.color = new Color(0.35f, 0.35f, 0.35f, 1.0f);
        for (int i = 0; i < material.Length; i++)
            material[i].SetColor("_Color", new Color(0.35f, 0.35f, 0.35f, 1.0f));
    }

    /// <summary>
    /// Bật hoặc tắt hiển thị mô hình món hàng.
    /// </summary>
    public void SetVisible(bool visible)
    {
        meshRenderer.enabled = visible;
    }

    /// <summary>
    /// Khôi phục màu sắc gốc cho món hàng.
    /// </summary>
    public void SetColorShow()
    {
        // material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        for (int i = 0; i < material.Length; i++)
            material[i].SetColor("_Color", new Color(1.0f, 1.0f, 1.0f, 1.0f));
    }


    /// <summary>
    /// Chọn món hàng và giải phóng ô chứa hiện tại trên kệ.
    /// </summary>
    public void SelectWares()
    {
        //dọn ô cũ trên kệ
        oldIndex = shelfController.shelfSlotList[0].waresInRowSlot.IndexOf(this);
        shelfController.shelfSlotList[0].waresInRowSlot[oldIndex] = null;
        //highlightEffect.highlighted = true;
    }

    /// <summary>
    /// Bỏ chọn món hàng và đặt lại vào vị trí cũ.
    /// </summary>
    public void UnSelectWares()
    {
        //đặt lại vào ô cũ
        shelfController.shelfSlotList[0].waresInRowSlot[oldIndex] = this;
        //highlightEffect.highlighted = false;
    }

    /// <summary>
    /// Di chuyển món hàng trở lại vị trí ban đầu trên kệ.
    /// </summary>
    public void MoveBack()
    {
        //transform.localPosition = FindPosInShelf();
        transform.DOLocalMove(FindPosInShelf(), 0.2f).SetEase(Ease.Linear).SetDelay(0.0f).OnComplete(() =>
        {
            //highlightEffect.highlighted = false;

        });

    }

    /// <summary>
    /// Tính toán vị trí chính xác của món hàng trên kệ dựa vào colID và deptID.
    /// </summary>
    private Vector3 FindPosInShelf()
    {
        Vector3 pos = Vector3.zero;
        pos = new Vector3((2 * colID + 1) * 0.5f * GameManager.Instance.gamePlaySetting.tileSizeX, 0.0f, -deptID * GameManager.Instance.gamePlaySetting.tileSizeZ);
        return pos;
    }

    /// <summary>
    /// Di chuyển món hàng sang kệ khác tại ô trống được chỉ định.
    /// </summary>
    public void MoveToAnotherShelf(ShelfController shelf, int emptySlot)
    {
        //đưa tới ô mới của kệ khác
        transform.SetParent(shelf.container.transform);
        shelf.shelfSlotList[0].waresInRowSlot[emptySlot] = this;
        //tọa độ mới
        colID = emptySlot;
        deptID = 0;
        //bắt đầu di chuyển về vị trí mới
        shelfController = shelf;
        transform.DOMove(shelfController.shelfSlotList[0].warePointList[emptySlot].position, 0.1f);
        //highlightEffect.highlighted = false;
    }

    /// <summary>
    /// Xóa hoàn toàn món hàng khỏi scene.
    /// </summary>
    public void RemoveItem()
    {
        Destroy(gameObject);
    }
}
