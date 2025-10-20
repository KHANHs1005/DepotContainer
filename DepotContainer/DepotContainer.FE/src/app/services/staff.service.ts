import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment'; // ✅ import environment

@Injectable({
  providedIn: 'root'
})
export class StaffService {
  private apiUrl = `${environment.apiUrl}/Staff`; // ⚠️ endpoint backend trả danh sách nhân viên

  constructor(private http: HttpClient) {}

  getAllStaffs(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }
}
