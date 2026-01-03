import { Routes } from '@angular/router';
import { LoginComponent } from './Page/login/login';
import { DashboardComponent } from './Page/dashboard/dashboard';  
import { BookingListComponent } from './Page/booking/booking';
import { BlockManagementComponent } from './Page/block-management/block-management';
import { SlotManagementComponent } from './Page/slotmanagement/slotmanagement';
import { EirManagementComponent } from './Page/eir-manager/eir-manager';
import { ContainerComponent } from './Page/container/container';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'blocks/:id/slots', component: SlotManagementComponent },
  { path: 'block-management', component: BlockManagementComponent },
  { path: 'booking', component: BookingListComponent },
  { path: 'eir-manager', component: EirManagementComponent },
  { path: 'container', component: ContainerComponent }
];
