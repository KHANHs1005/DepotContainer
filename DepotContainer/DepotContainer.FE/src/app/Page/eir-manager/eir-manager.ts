import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SidebarComponent } from '../../layout/sidebar/sidebar';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-eir-management',
  templateUrl: './eir-manager.html',
  standalone: true,
  imports: [CommonModule, SidebarComponent],
  styleUrls: ['./eir-manager.css']
})
export class EirManagementComponent implements OnInit {
  apiUrl = 'http://localhost:5011/api/Eir'; // 🔗 URL API thật
  activeTab = 'eir';

  eirList: any[] = [];
  eir: any = {
    dateCreated: '',
    type: '',
    status: '',
    containerNumber: '',
    sizeType: '',
    weight: '',
    operator: '',
    location: ''
  };
  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.loadEirs();
  }

  loadEirs() {
    this.http.get(this.apiUrl).subscribe({
      next: (data: any) => this.eirList = data,
      error: err => console.error('Lỗi load danh sách EIR:', err)
    });
  }

  createEir() {
    this.http.post(this.apiUrl, this.eir).subscribe({
      next: (res) => {
        alert('Tạo EIR thành công!');
        this.loadEirs();
      },
      error: (err) => console.error('Lỗi tạo EIR:', err)
    });
  }

  viewEir(item: any) {
    alert(`Chi tiết EIR: \nSố EIR: ${item.eirNo}\nContainer: ${item.containerNumber}`);
  }
}
