using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story
{
    public int id;
    public string content;

    public Story(int newid, string newContent)
    {
        id = newid;
        content = newContent;
    }
}
public class StoryDatabase : MonoBehaviour
{
    public static List<Story> Stories = new List<Story>();

    private void Awake()
    {
        Stories.Add(new Story(0, "Câu chuyện Lịch sử Nhà Tây Sơn"));
        Stories.Add(new Story(1, "Câu chuyện Lịch sử Nguyễn Huệ"));
        Stories.Add(new Story(2, "Câu chuyện Lịch sử Nguyễn Nhạc"));
        Stories.Add(new Story(3, "Câu chuyện Lịch sử Nguyễn Lữ"));
        Stories.Add(new Story(4, "Câu chuyện Lịch sử Trận tập kích"));
        Stories.Add(new Story(5, "Câu chuyện Lịch sử Phạm Văn Tham"));
        Stories.Add(new Story(6, "Câu chuyện Lịch sử Ngô Thì Nhậm"));
        Stories.Add(new Story(7, "Câu chuyện Lịch sử Phòng tuyến Tam Điệp"));
        Stories.Add(new Story(8, "Câu chuyện Lịch sử Trần Quang Toản"));
        Stories.Add(new Story(9, "Câu chuyện Lịch sử Bảo vệ Tây Sơn"));
        Stories.Add(new Story(10, "Câu chuyện Lịch sử Đại chiến quân Thanh"));
    }
}
