import { Routes } from '@angular/router';
import { LoginComponent } from './Page/login/login';
import { DashboardComponent } from './Page/dashboard/dashboard';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'dashboard', component: DashboardComponent }
];
