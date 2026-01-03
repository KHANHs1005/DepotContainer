import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './sidebar.html',
  styleUrls: ['./sidebar.css']
})
export class SidebarComponent {
  staffName = '';

  constructor() {
    const staff = JSON.parse(localStorage.getItem('staff') || '{}');
    this.staffName = staff.staffName || 'Nhân viên';
  }

  logout() {
    localStorage.removeItem('staff');
    window.location.href = '/login';
  }
}
