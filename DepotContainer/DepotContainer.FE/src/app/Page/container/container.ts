import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; // ✅ Thêm để dùng [(ngModel)]
import { ContainerService, Container } from '../../services/container.service';
import { SidebarComponent } from '../../layout/sidebar/sidebar';

@Component({
  selector: 'app-container-management',
  standalone: true,
  imports: [CommonModule, FormsModule, SidebarComponent],
  templateUrl: './container.html',
  styleUrls: ['./container.css']
})
export class ContainerComponent implements OnInit {
  containers: Container[] = [];
  searchTerm: string = ''; // 🔍 Từ khóa tìm kiếm

  constructor(private containerService: ContainerService) { }

  ngOnInit() {
    this.loadContainers();
  }

  /** 🔹 Tải danh sách container ban đầu */
  loadContainers() {
    this.containerService.getAll().subscribe({
      next: (res) => (this.containers = res),
      error: (err) => console.error('Lỗi khi tải dữ liệu container:', err)
    });
  }

  /** ➕ Thêm mới container */
  onAddNew() {
    const newContainer: Container = {
      containerNumber: prompt('Nhập mã container mới:') || '',
      containerType: prompt('Nhập loại container:') || '',
      bookingNumber: prompt('Nhập số booking:') || ''
    };

    // Nếu thiếu thông tin thì dừng
    if (!newContainer.containerNumber.trim()) {
      alert('⚠️ Bạn phải nhập mã container!');
      return;
    }

    this.containerService.create(newContainer).subscribe({
      next: () => {
        alert('✅ Đã thêm container mới!');
        this.loadContainers();
      },
      error: (err) => console.error('❌ Lỗi thêm container:', err)
    });
  }

  /** 📤 Xuất danh sách container ra file Excel */
  onExport() {
    if (this.containers.length === 0) {
      alert('⚠️ Không có dữ liệu để xuất!');
      return;
    }

    const header = 'Mã container,Loại container,Số booking\n';
    const rows = this.containers
      .map(
        (c) =>
          `${c.containerNumber || ''},${c.containerType || ''},${c.bookingNumber || ''
          }`
      )
      .join('\n');

    const csvContent = header + rows;
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    const link = document.createElement('a');
    link.href = URL.createObjectURL(blob);
    link.download = 'containers.csv';
    link.click();

    alert('📂 Dữ liệu đã được xuất ra file containers.csv');
  }
  onSearch() {
    const keyword = this.searchTerm.trim().toLowerCase();

    if (!keyword) {
      alert('⚠️ Vui lòng nhập thông tin trước khi tìm kiếm!');
      return;
    }

    this.containerService.getAll().subscribe({
      next: (res) => {
        this.containers = res.filter(
          (c) =>
            c.containerNumber?.toLowerCase().includes(keyword) ||
            c.containerType?.toLowerCase().includes(keyword) ||
            c.bookingNumber?.toLowerCase().includes(keyword)
        );

        if (this.containers.length === 0) {
          alert('❗Không tìm thấy container nào phù hợp!');
        }
      },
      error: (err) => console.error('❌ Lỗi tìm kiếm:', err)
    });
  }
}
