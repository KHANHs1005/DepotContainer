import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class LoginComponent {
  username = '';
  password = '';
  errorMessage = '';

  constructor(private http: HttpClient, private router: Router) { }

  onLogin() {
    const body = {
      username: this.username,
      password: this.password
    };

    this.http.post<any>('http://localhost:5011/api/Auth/login', body)
      .subscribe({
        next: (res) => {
          localStorage.setItem('staff', JSON.stringify(res));
          alert(res.message);
          this.router.navigate(['/dashboard']);
        },
        error: (err) => {
          this.errorMessage = err.error?.message || 'Đăng nhập thất bại';
        }
      });
  }
}
