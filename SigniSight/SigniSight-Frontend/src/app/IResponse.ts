import { ITranslations } from "./ITranslations";

export interface IResponse{
    detectLanguage:Object;
    translations:ITranslations[];
}