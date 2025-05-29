using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tập hợp các cài đặt gameplay ở dạng ScriptableObject giúp designer dễ tùy chỉnh.
/// </summary>

[System.Serializable]
[CreateAssetMenu(fileName = "GamePlaySetting")]
public class GamePlaySetting : ScriptableObject
{
    [Header("Kích thước bàn chơi")]
    /// <summary>
    /// Số cột tối đa của kệ chứa vật phẩm.
    /// </summary>
    [Tooltip("Số cột tối đa của kệ")]
    public int colSizeMax;

    /// <summary>
    /// Số hàng sâu tối đa của kệ.
    /// </summary>
    [Tooltip("Số hàng sâu tối đa")]
    public int deptSizeMax;

    /// <summary>
    /// Kích thước ô theo trục X, ảnh hưởng tới khoảng cách giữa các vật phẩm.
    /// </summary>
    [Tooltip("Độ rộng mỗi ô")] 
    public float tileSizeX;

    /// <summary>
    /// Kích thước ô theo trục Y.
    /// </summary>
    [Tooltip("Độ cao mỗi tầng")]
    public float tileSizeY;

    /// <summary>
    /// Kích thước ô theo trục Z.
    /// </summary>
    [Tooltip("Độ sâu mỗi ô")]
    public float tileSizeZ;

    [Header("Tiến trình & Phần thưởng")]
    /// <summary>
    /// Số sao cần để mở rương thưởng.
    /// </summary>
    [Tooltip("Số sao mở rương")]
    public int openChestNumber;

    /// <summary>
    /// Số trận thắng cần đạt để mở vòng quay may mắn.
    /// </summary>
    [Tooltip("Mốc quay vòng quay may mắn")]
    public int luckySpinProgressMax;

    /// <summary>
    /// Số trận thắng cần để nhận thưởng chiến thắng.
    /// </summary>
    [Tooltip("Mốc thưởng chiến thắng")]
    public int rewardVictoryProgressMax;

    [Header("Xếp hạng")]
    /// <summary>
    /// Điểm kinh nghiệm cần có để lên một bậc hạng.
    /// </summary>
    [Tooltip("Điểm kinh nghiệm mỗi cấp")]
    public int rankStep;

    /// <summary>
    /// Số xu thưởng thêm khi thăng hạng.
    /// </summary>
    [Tooltip("Xu thưởng khi thăng hạng")]
    public int rankRewardBonus;

    /// <summary>
    /// Số lượng người chơi hiển thị trong bảng xếp hạng.
    /// </summary>
    [Tooltip("Số dòng của bảng xếp hạng")]
    public int rankingSize;

    [Header("Danh sách phần thưởng")]
    [Tooltip("Gói quà khi mở lần đầu")]
    public List<RewardModel> rewardPackage1;

    [Tooltip("Gói quà khi mở lần hai")]
    public List<RewardModel> rewardPackage2;

    [Tooltip("Gói quà khi mở lần ba")]
    public List<RewardModel> rewardPackage3;

    [Tooltip("Phần thưởng khi mở rương")]
    public List<RewardModel> openChestReward;

    [Tooltip("Phần thưởng khi mở tem mới")]
    public List<RewardModel> unlockStampReward;
}
