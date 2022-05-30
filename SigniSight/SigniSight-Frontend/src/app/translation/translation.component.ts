import { Component, OnInit } from "@angular/core";
import { Router } from '@angular/router';
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

  translate():string{
    this.translationService.translation(this.textToTranslate, this.endLanguageCode)
    .subscribe((data) =>{
      //console.log(data)
      //console.log(this.translationService.response)//[0].translations[0].translatedText[0].text)
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

  imageUrl:string = "";

  ocr():void{
    this.translationService.recognition(this.imageUrl)
    .subscribe((data) =>{
      console.log(data)
      // If we successfully login, let's redirect to the home page
      //this.router.navigate(['home'])
    },
    (error) =>{
      console.log(error)
      // Makes error message appear through ngIf
      this.error = true;
    })
    //return this.translationService.text;
  }

  constructor(private translationService:TranslationService, private router:Router) { }
  ngOnInit(): void {
  }

}
