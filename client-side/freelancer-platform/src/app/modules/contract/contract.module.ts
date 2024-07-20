import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { ContractManagementComponent } from './contract-management/contract-management.component';
import { RouterModule, Routes } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MyContractsComponent } from './my-contracts/my-contracts.component';
import { AuthGuard } from '../auth/helpers/auth.guard';
import { RoleGuard } from '../auth/helpers/role.guard';
import { MatSortModule } from '@angular/material/sort';
import { MatMenuModule } from '@angular/material/menu';

export const contractRoutes: Routes = [
  { path: 'job/:id/contract-management', component: ContractManagementComponent, canActivate: [AuthGuard, RoleGuard], data: { roles: ['CLIENT']} },
  { path: 'my-contracts', component: MyContractsComponent, canActivate: [AuthGuard] }
];

@NgModule({
  declarations: [
    ContractManagementComponent,
    MyContractsComponent
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    MatTableModule,
    RouterModule,
    MatIconModule,
    MatSortModule,
    MatMenuModule,
    MatButtonModule
  ]
})
export class ContractModule { }
