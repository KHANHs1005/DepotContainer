import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './Page/login/login';
import { DashboardComponent } from './Page/dashboard/dashboard';
import { EirManagementComponent } from './Page/eir-manager/eir-manager';
import { ContainerComponent } from './Page/container/container';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'eir', component:EirManagementComponent},
  { path: 'container',component:ContainerComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
