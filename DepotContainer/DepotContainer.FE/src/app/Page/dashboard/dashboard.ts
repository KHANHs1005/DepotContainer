import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { SidebarComponent } from '../../layout/sidebar/sidebar';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule ,SidebarComponent],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.css']
})
export class DashboardComponent implements OnInit {
  staffName = '';
  stats = {
    emptyContainer: 0,
    booking: 0,
    eir: 0,
    slot: 0
  };
  operatorName: any[] = [];
  loading = true;
  error = '';

  constructor(
    private http: HttpClient,
    private router: Router
  ) { }

  ngOnInit() {
    const staff = JSON.parse(localStorage.getItem('staff') || '{}');
    this.staffName = staff.staffName || 'Nhân viên';
    this.loadStatistics();
  }

  loadStatistics() {
    this.loading = true;
    this.error = '';

    this.http.get<any>('http://localhost:5011/api/Statistics')
      .subscribe({
        next: (data) => {
          if (data?.summary || data?.Summary) {
            const s = data.summary || data.Summary;
            this.stats = {
              emptyContainer: s.emptyContainer || s.EmptyContainer || 0,
              booking: s.booking || s.Booking || 0,
              eir: s.eir || s.Eir || 0,
              slot: s.slot || s.Slot || 0
            };
          }

          if (data.OperatorName || data.operatorName) {
            const arr = data.OperatorName || data.operatorName;
            this.operatorName = arr.map((item: any) => ({
              name: item.OperatorName || item.operatorName,
              import: item.Import || item.import,
              export: item.Export || item.export,
              stock0to10: item.Stock0To10 || item.stock0To10,
              stock10plus: item.Stock10Plus || item.stock10Plus
            }));
          }

          this.loading = false;
        },
        error: (err) => {
          console.error('❌ Lỗi khi gọi API:', err);
          this.error = 'Không thể tải dữ liệu.';
          this.loading = false;
        }
      });
  }

  // ✅ Thêm hàm này
  logout() {
    localStorage.removeItem('staff');
    this.router.navigate(['/login']);
  }
  goToBlockManagement() {
    this.router.navigate(['/block-management']);
  }
  goToBooking() {
    this.router.navigate(['/booking']);
  }
}
