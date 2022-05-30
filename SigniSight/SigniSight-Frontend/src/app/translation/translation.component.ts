import { Component, OnInit } from "@angular/core";
import { Router } from '@angular/router';
import { Subject } from "rxjs";
import { IResponse } from "../IResponse";
import { TranslationService } from '../translation.service';

@Component({
  selector: "translation",
  templateUrl: "./translation.component.html",
  styleUrls: ["./translation.component.css"]
})
export class TranslationComponent implements OnInit {

  textToTranslate:string = "";
  endLanguageCode:string = "";
  error:boolean = false;

  //arr:IResponse[]= [];
  //subject:Subject<IResponse[]> = new Subject<IResponse[]>();

  onSubmit():string{
    this.translationService.translation(this.textToTranslate, this.endLanguageCode)
    .subscribe((data) =>{
      console.log(data)
      //console.log(this.translationService.response)//[0].translations[0].translatedText[0].text)
      //let translatedText:string = data[0].translations[0].text;
      //this.arr = data;
      // Let's store the data in our service's string
      this.translationService.text = data[0].translations[0].text;
      console.log(this.translationService.text)
      // If we successfully login, let's redirect to the home page
      //this.router.navigate(['home'])
    },
    (error) =>{
      console.log(error)
      // Makes error message appear through ngIf
      this.error = true;
    })
    return this.translationService.text;
  }
  //Inject login service to component to use methods
  // Inject router for navigation
  constructor(private translationService:TranslationService, private router:Router) { }

  ngOnInit(): void {
  }

}
