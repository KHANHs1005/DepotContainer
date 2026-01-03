import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';

interface Container {
  containerId: number;
  containerNumber?: string;
  contStatus?: string;
  contCondition?: string;
  weight?: number | null;
  timeIn?: string;
}

interface Slot {
  slotId: number;
  bay: number;
  row: number;
  tier: number;
  container: Container | null;
}

interface BayRow {
  bay: number;
  row: number;
}

@Component({
  selector: 'app-slot-management',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './slotmanagement.html', // ✅ sửa đúng tên file HTML
  styleUrls: ['./slotmanagement.css']   // ✅ sửa đúng tên file CSS
})
export class SlotManagementComponent implements OnInit {
  blockId!: number;
  blockName = '';
  slots: Slot[] = [];
  bayRowCombinations: BayRow[] = [];
  selectedBayRow: BayRow | null = null;
  tiersInSelected: { tierNumber: number; container: Container | null }[] = [];
  showContainerModal = false;
  selectedContainer: Container | null = null;

  private readonly apiBase = 'http://localhost:5011';

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private router: Router
  ) { }

  ngOnInit(): void {
    // ✅ Lấy blockId từ URL (vd: /blocks/10/slots)
    this.blockId = Number(this.route.snapshot.paramMap.get('id'));
    this.blockName = `Block ${this.blockId}`;
    console.log('🧱 BlockId:', this.blockId);
    this.loadSlots();
  }

  /** Gọi API lấy dữ liệu slots */
  loadSlots(): void {
    const apiUrl = `${this.apiBase}/api/Block/${this.blockId}/slots`;
    console.log('🔗 Gọi API:', apiUrl);

    this.http.get<Slot[]>(apiUrl).subscribe({
      next: (data) => {
        this.slots = data;

        // ✅ Tạo danh sách Bay–Row duy nhất từ slots
        const seen = new Set<string>();
        this.bayRowCombinations = [];

        data.forEach(slot => {
          const key = `${slot.bay}-${slot.row}`;
          if (!seen.has(key)) {
            seen.add(key);
            this.bayRowCombinations.push({ bay: slot.bay, row: slot.row });
          }
        });

        console.log('✅ Bay–Row combinations:', this.bayRowCombinations);
      },
      error: (err) => {
        console.error('❌ Lỗi load slots:', err);
      }
    });
  }

  /** Khi click chọn 1 ô Bay–Row */
  selectBayRow(bayRow: BayRow): void {
    this.selectedBayRow = bayRow;

    // Lọc ra tất cả slot có bay–row tương ứng
    const slotsForBayRow = this.slots.filter(s => s.bay === bayRow.bay && s.row === bayRow.row);

    // Hiển thị theo thứ tự tier giảm dần (cao xuống thấp)
    this.tiersInSelected = slotsForBayRow
      .map(s => ({ tierNumber: s.tier, container: s.container }))
      .sort((a, b) => b.tierNumber - a.tierNumber);

    console.log('📦 Tiers in selected bay–row:', this.tiersInSelected);
  }

  /** Hiển thị chi tiết container */
  showContainerDetails(container: Container) {
    this.selectedContainer = container;
    this.showContainerModal = true;
  }

  /** Đóng modal */
  closeContainerModal() {
    this.showContainerModal = false;
    this.selectedContainer = null;
  }

  /** Quay lại trang quản lý block */
  goBack() {
    this.router.navigate(['/block-management']);
  }
}
