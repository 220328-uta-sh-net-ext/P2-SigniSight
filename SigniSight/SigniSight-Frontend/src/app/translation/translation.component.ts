import { Component, OnInit } from "@angular/core";
import { Router } from '@angular/router';
import { TranslationService } from '../translation.service';

@Component({
  selector: "translation",
  templateUrl: "./translation.component.html",
  styleUrls: ["./translation.component.css"],
})
export class TranslationComponent implements OnInit {

  textToTranslate:string = "";
  endLanguageCode:string = "";
  error:boolean = false;

  onSubmit():void{
    this.translationService.translation(this.textToTranslate, this.endLanguageCode)
    .subscribe((data) =>{
      console.log(data)
      // Let's store the data in our service's string
      this.translationService.translatedText = data.translatedText;
      console.log(this.translationService.translatedText)
      // If we successfully login, let's redirect to the home page
      //this.router.navigate(['home'])
    },
    (error) =>{
      console.log(error)
      // Makes error message appear through ngIf
      this.error = true;
    })
  }

  //Inject login service to component to use methods
  // Inject router for navigation
  constructor(private translationService:TranslationService, private router:Router) { }

  ngOnInit(): void {
  }

}
