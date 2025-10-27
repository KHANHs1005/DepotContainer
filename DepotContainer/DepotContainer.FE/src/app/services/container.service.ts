import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Container {
  containerId?: number;
  containerNumber: string;
  operatorName?: string;
  size?: string;
  bookingNumber?: string;
  containerType: string;
  contStatus?: string;
  contCondition?: string;
  currentBlock?: string;
  weight?: number;
}

@Injectable({
  providedIn: 'root'
})
export class ContainerService {
  private apiUrl = 'http://localhost:5011/api/Container';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Container[]> {
    return this.http.get<Container[]>(this.apiUrl);
  }

  getById(id: number): Observable<Container> {
    return this.http.get<Container>(`${this.apiUrl}/${id}`);
  }

  create(data: any): Observable<any> {
    return this.http.post(this.apiUrl, data);
  }

  update(data: any): Observable<any> {
    return this.http.put(this.apiUrl, data);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
