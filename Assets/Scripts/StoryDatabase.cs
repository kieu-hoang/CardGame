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
        Stories.Add(new Story(0, "Câu chuyện Lịch sử"));
        Stories.Add(new Story(1, "Câu chuyện Lịch sử - Nhà Tây Sơn \nNhà Tây Sơn (Tây Sơn triều) là một triều đại quân chủ trong lịch sử Việt Nam tồn tại từ năm 1778 đến năm 1802, được thành lập trong bối cảnh tranh chấp quyền lực cuối thời Lê Trung hưng (1533–1789). " +
        "Theo cách gọi của phần lớn sử gia, nhất là các sử gia hiện đại tại Việt Nam thì \"nhà Tây Sơn\" được dùng để gọi triều đại của anh em Nguyễn Nhạc, Nguyễn Lữ và Nguyễn Huệ để phân biệt với nhà Nguyễn của Nguyễn Ánh (vì cùng họ Nguyễn). " +
            "Ngoài ra, \"Tây Sơn\" cũng chỉ các lãnh tụ và quân đội khởi nghĩa xuất thân từ ấp Tây Sơn; cũng được dùng làm tên cuộc chiến của Tây Sơn."));
        Stories.Add(new Story(2, "Câu chuyện Lịch sử - Nguyễn Nhạc \nNguyễn Nhạc (1743 – 1793) hay còn gọi là Nguyễn Văn Nhạc, là vị hoàng đế sáng lập ra Nhà Tây Sơn, ở ngôi hoàng đế từ năm 1778 đến năm 1788, lấy niên hiệu là Thái Đức thường gọi là Thái Đức Đế." +
                                 " Từ năm 1789 – 1793, ông từ bỏ đế hiệu để nhường ngôi hoàng đế cho em trai là hoàng đế Quang Trung, còn ông tự hạ tước hiệu của mình xuống thành Tây Sơn vương.\nNguyễn Văn Nhạc và hai người em trai của ông, được biết với tên gọi Anh em Tây Sơn," +
                                 " là những lãnh đạo của cuộc khởi nghĩa Tây Sơn đã chấm dứt cuộc nội chiến kéo dài giữa hai tập đoàn phong kiến Trịnh ở phía Bắc và Nguyễn ở phía Nam, lật đổ hai tập đoàn này cùng Nhà Hậu Lê."));
        Stories.Add(new Story(3, "Câu chuyện Lịch sử - Nguyễn Huệ \nQuang Trung Hoàng đế (1753 – 1792), danh xưng khác là Bắc Bình Vương, tên khai sinh là Hồ Thơm, quê gốc Nghệ An sau đổi tên thành Nguyễn Huệ, Nguyễn Quang Bình, là một nhà chính trị, " +
                                 "nhà quân sự người Việt Nam, vị hoàng đế thứ 2 của Nhà Tây Sơn, sau khi Thái Đức Hoàng đế Nguyễn Nhạc thoái vị và nhường ngôi cho ông. Nguyễn Huệ còn là người đánh bại các cuộc xâm lược Đại Việt của Xiêm La từ phía nam, " +
                                 "của Đại Thanh từ phía bắc; đồng thời còn là vị vua có tài cai trị khi đề ra nhiều kế hoạch cải cách tiến bộ xây dựng Đại Việt."));
        Stories.Add(new Story(4, "Câu chuyện Lịch sử - Nguyễn Lữ \nNguyễn Lữ (1754–1787) hay còn gọi là Nguyễn Văn Lữ là một chính trị gia và nhà quân sự Việt Nam ở thế kỷ 18. Ông là 1 trong 3 thủ lĩnh của phong trào Tây Sơn và là em của vua Thái Đức Hoàng đế Nguyễn Nhạc" +
                                 " và Quang Trung Hoàng đế Nguyễn Huệ nhà Tây Sơn, một trong những triều đại hiển hách nhất về võ công của Việt Nam."));
        Stories.Add(new Story(5, "Câu chuyện Lịch sử - Nguyễn Quang Toản \nNguyễn Quang Toản (1783 – 1802), là vị hoàng đế thứ 3 và cuối cùng của Vương triều Tây Sơn trong lịch sử Việt Nam. Ông là con trai của vua Quang Trung (Nguyễn Huệ). " +
                                 "Quang Toản sau khi lên ngôi vì nhỏ tuổi, không có kinh nghiệm cai quản triều chính nên đã bị cậu ruột là Thái sư Bùi Đắc Tuyên chuyên quyền thâu tóm triều chính. Nội bộ Tây Sơn cũng từ đó mà lục đục, suy yếu, các tướng lĩnh tranh chấp quyền hành giết hại lẫn nhau. Nhân lúc Tây Sơn suy yếu, " +
                                 "chúa Nguyễn ở Gia Định thừa cơ bắc phạt, sau 10 năm thì khôi phục sơn hà Đàng Trong, vua tôi Tây Sơn chạy ra miền bắc. Năm 1802, quân nhà Nguyễn tiến ra Bắc Hà, Cảnh Thịnh cùng triều đình đều bị bắt và đưa về xử lăng trì tại Huế. Cái chết của ông cũng đánh dấu sự chấm hết của nhà Tây Sơn."));
        Stories.Add(new Story(6, "Câu chuyện Lịch sử - Ngô Thì Nhậm \nNgô Thì Nhậm (còn gọi là Ngô Thời Nhiệm - 1746 –  1803), là một tu sỹ Phật giáo, danh sĩ, nhà văn đời Hậu Lê và Tây Sơn, người có công lớn trong việc giúp triều Tây Sơn đánh lui quân Thanh. Ngô Thì Nhậm chính là người chủ trì về các " +
                                 "chính sách và giao dịch ngoại giao với Trung Hoa. Ông là người đứng đầu một trong những sứ bộ ngoại giao sang Trung Hoa. Vốn là người văn chương nức tiếng bấy giờ, phần lớn các thư từ bang giao giữa Đại Việt và nhà Thanh đều do chính tay ông soạn thảo. Những văn kiện ngoại giao của Ngô Thì Nhậm" +
                                 " thể hiện rõ nguyên tắc về chủ quyền lãnh thổ, danh dự quốc gia với các chính sách mềm dẻo, linh hoạt, cứng rắn. Chính sách ngoại giao khôn khéo đối với nhà Thanh là tác phẩm của Ngô Thì Nhậm."));
        Stories.Add(new Story(7, "Câu chuyện Lịch sử - Phạm Văn Tham \nPhạm Văn Tham hay còn được gọi là Phạm Văn Sâm là một tướng lĩnh của phong trào Tây Sơn. Phạm Văn Tham có chị lấy vua Thái Đức Nguyễn Nhạc. Ông là anh của Hộ giá Thượng tướng quân Phạm Ngạn. Năm 1771, Tham gia khởi nghĩa Tây Sơn, ông lập được" +
                                 " nhiều công lao nên được phong chức Thái bảo. Ông tham gia nhiều trận đánh ở miền nam trung bộ và Gia Định, giết các tướng Dương Công Trừng, Nguyễn Đăng Vân, đánh bại các tướng Tôn Thất Huy, Tôn Thất Hội."));
        Stories.Add(new Story(8, "Câu chuyện Lịch sử - Phòng tuyến Tam Điệp \nNăm 1788, 29 vạn quân Thanh kéo sang Việt Nam, với lý do diệt Tây Sơn dựng lại nhà Hậu Lê. Ngô Thì Nhậm dùng kế chọn đèo Tam Điệp làm căn cứ quân sự ngăn cản quân Thanh. " +
                                 "Theo đó, Ngô Văn Sở, Phan Văn Lân, Ninh Tốn rút quân từ Thăng Long về Tam Điệp. Đây là một nơi có vị trí khá hiểm trở, núi non hùng vĩ như bức tường thành án ngữ giữa hai miền. Đồi núi, thung lũng liên hoàn tạo thành khối vững chắc án ngữ Bắc-Nam, giúp Nguyễn Huệ công thủ, tiến thoái cất lương, giấu " +
                                 "quân để mùa xuân kỷ dậu (1789) tiến ra kinh thành Thăng Long quét sạch 20 vạn quân Thanh viết nên trang sử vẻ vang của dân tộc Việt Nam."));
    }
}
