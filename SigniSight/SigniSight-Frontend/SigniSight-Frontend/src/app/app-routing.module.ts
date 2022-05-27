import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { HomePageComponent } from "./home-page/home-page.component";
import { Router, RouterModule } from "@angular/router";
import { RosterComponent } from "./roster/roster.component";
import { TranslationComponent } from "./translation/translation.component";

const routes = [
  { path: "", redirectTo: "/home", pathMatch: "full" },
  { path: "home", component: HomePageComponent },
  { path: "roster", component: RosterComponent },
  { path: "translation", component: TranslationComponent },
];

@NgModule({
  imports: [CommonModule, RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}