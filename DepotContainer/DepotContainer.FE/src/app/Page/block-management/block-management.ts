import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SidebarComponent } from '../../layout/sidebar/sidebar';

@Component({
  selector: 'app-block-management',
  standalone: true,
  imports: [CommonModule, FormsModule, SidebarComponent],
  templateUrl: './block-management.html',
  styleUrls: ['./block-management.css']
})
export class BlockManagementComponent implements OnInit {
  blocks: any[] = [];
  showAddModal = false;
  newBlock = {
    blockName: '',
    totalSlots: 0
  };
  staffName = localStorage.getItem('staff') || 'Admin';
  apiUrl = 'http://localhost:5011/api/Block';

  constructor(
    private http: HttpClient,
    private router: Router
  ) { }

  ngOnInit() {
    this.loadBlocks();
  }

  // 🟩 Lấy danh sách Block
  loadBlocks() {
    this.http.get<any[]>(this.apiUrl)
      .subscribe({
        next: (data) => {
          console.log('✅ Blocks:', data);
          this.blocks = data;
        },
        error: (err) => {
          console.error('❌ Lỗi load block:', err);
          alert('Không thể tải danh sách block.');
        }
      });
  }

  // 🟩 Mở modal thêm Block
  openAddBlockModal() {
    this.showAddModal = true;
  }

  closeAddBlockModal() {
    this.showAddModal = false;
    this.newBlock = { blockName: '', totalSlots: 0 };
  }

  // 🟩 Thêm Block mới
  addBlock() {
    this.http.post(this.apiUrl, this.newBlock)
      .subscribe({
        next: () => {
          alert('Thêm block thành công!');
          this.closeAddBlockModal();
          this.loadBlocks();
        },
        error: (err: HttpErrorResponse) => {
          console.error('❌ Lỗi thêm block:', err);
          alert(`Không thể thêm block: ${err.error?.message || err.statusText}`);
        }
      });
  }

  // 🟥 Xóa Block
  deleteBlock(id: number) {
    if (!confirm('Bạn có chắc muốn xóa block này?')) return;

    this.http.delete(`${this.apiUrl}/${id}`, { responseType: 'text' })
      .subscribe({
        next: (res) => {
          console.log('✅ Xóa thành công:', res);
          alert('Xóa thành công!');
          this.loadBlocks();
        },
        error: (err: HttpErrorResponse) => {
          console.error('❌ Lỗi xóa block:', err);
          // Hiển thị chi tiết lỗi server thay vì chỉ err.message
          alert(`Không thể xóa block (mã lỗi ${err.status}): ${err.error || err.message}`);
        }
      });
  }

  manageBlock(id: number) {
    this.router.navigate(['/blocks', id, 'slots']);
  }

  goToDashboard() {
    this.router.navigate(['/dashboard']);
  }

  goToBlockManagement() {
    this.router.navigate(['/block-management']);
  }

  logout() {
    localStorage.removeItem('staff');
    this.router.navigate(['/login']);
  }
}
