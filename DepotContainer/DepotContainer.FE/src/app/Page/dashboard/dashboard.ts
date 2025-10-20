import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class DashboardComponent implements OnInit {
  staffName = '';
  stats = { emptyContainer: 0, booking: 0, eir: 0, slot: 0 };
  shippingStats: any[] = [];

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit() {
    const staff = JSON.parse(localStorage.getItem('staff') || '{}');
    this.staffName = staff.staffName || 'Nhân viên';

    this.http.get<any>('http://localhost:5011/api/Booking')
      .subscribe((data) => {
        this.stats = data.summary;
        this.shippingStats = data.shippingLines;
      });
  }
    
  logout() {
    localStorage.removeItem('staff');
    this.router.navigate(['/login']);
  }
}
