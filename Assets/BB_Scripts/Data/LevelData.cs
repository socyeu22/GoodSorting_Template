using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "LevelData")]

/// <summary>
/// ScriptableObject lưu trữ thông tin cấu hình cho từng màn chơi.
/// </summary>
public class LevelData : ScriptableObject
{
    [Tooltip("ID duy nhất của màn chơi.")]
    public int ID;

    [Tooltip("ID của scene Unity tương ứng với màn này.")]
    public int sceneID;

    [Tooltip("Thời gian giới hạn (giây) để hoàn thành màn.")]
    public int time;

    [Tooltip("Tổng số sản phẩm cần tạo trong suốt màn.")]
    public int totalProductCount;

    [Tooltip("Số bộ sản phẩm xuất hiện ngẫu nhiên.")]
    public int dynamicProductSet;

    [Tooltip("Số lần đẩy cho phép trong một hành động.")]
    public int singlePush;

    [Tooltip("Số vòng kiểm tra ô trống trước khi kết thúc màn.")]
    public int roundsEmptyPlaceCount;
}
