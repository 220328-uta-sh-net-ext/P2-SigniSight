import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomePageComponent } from './home-page/home-page.component';
import { Router, RouterModule } from '@angular/router';
import { RosterComponent } from './roster/roster.component';

const routes = [
  {path: '', redirectTo: '/home', pathMatch:'full'},
    {path: 'home', component: HomePageComponent},
    {path: 'roster', component: RosterComponent}
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
