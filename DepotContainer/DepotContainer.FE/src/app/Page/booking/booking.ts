import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SidebarComponent } from '../../layout/sidebar/sidebar';
import { BookingService } from '../../services/booking.service';

@Component({
  selector: 'app-booking-list',
  standalone: true,
  imports: [CommonModule, SidebarComponent],
  templateUrl: './booking.html',
  styleUrls: ['./booking.css']
})
export class BookingListComponent implements OnInit {
  bookings: any[] = [];
  showDetail = false;
  selectedBooking: any = null;

  constructor(private bookingService: BookingService) {}

  ngOnInit() {
    this.bookingService.getAllBookings().subscribe({
      next: (data) => {
        this.bookings = data;
        console.log('✅ API Booking:', data);

      },
      error: (err) => {
        console.error('❌ Lỗi khi load booking:', err);
      }

    });
  }

  openDetail(booking: any) {
    console.log('Click Chi tiết:', booking);
    this.selectedBooking = booking;
    this.showDetail = true;
  }

  closeDetail() {
    this.showDetail = false;
  }

  createEIR() {
    alert('Tạo EIR cho ' + this.selectedBooking.bookingNo);
  }
}
