using Microsoft.Maui.Controls;

namespace dxclusive
{
	public partial class ClassPage3 : ContentPage
	{
		private readonly FirestoreService _firestoreService;
		private readonly string _selectedTerm;

		public ClassPage3(string selectedTerm)
		{
			InitializeComponent();
			_firestoreService = new FirestoreService();
			_selectedTerm = selectedTerm;
			TitleLabel.Text = $"{_selectedTerm}";

			// เรียกโหลดข้อมูล Classes เมื่อหน้าแสดงผล
			LoadClassesAsync();
		}

		private async void LoadClassesAsync()
		{
			try
			{
				// ดึงข้อมูล Classes ของ Term ที่เลือก
				var classes = await _firestoreService.GetClassDataAsync(_selectedTerm);

				// ลบปุ่มเดิมทั้งหมด (ถ้ามี)
				ButtonsContainer.Children.Clear();

				// สร้างปุ่มสำหรับแต่ละ Class
				foreach (var classData in classes)
				{
					var button = new Button
					{
						Text = classData["name"], // ใช้ชื่อของ Class
						FontSize = 20,
						BackgroundColor = Colors.White,
						TextColor = Colors.Black,
						CornerRadius = 10,
						HorizontalOptions = LayoutOptions.FillAndExpand,
					};

					// ตั้งค่าการกดปุ่มเพื่อแสดงรายละเอียด Class
					button.Clicked += async (sender, args) =>
			{
				var classDetails = await _firestoreService.GetClassDetailsAsync(_selectedTerm, classData["classId"]);
				var message = $"Name: {classDetails["name"]}\n" +
				              $"Teacher: {classDetails["teacher"]}\n" +  // Teacher แสดงก่อน
							  $"Description: {classDetails["description"]}"; // Description อยู่ด้านล่าง
				await DisplayAlert("Class Details", message, "OK");
			};


					// เพิ่มปุ่มเข้าไปใน Layout
					ButtonsContainer.Children.Add(button);
				}
			}
			catch (Exception ex)
			{
				await DisplayAlert("Error", $"Failed to load classes: {ex.Message}", "OK");
			}
		}
	}
}
