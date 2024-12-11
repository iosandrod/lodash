export class Comparer{
    Default:Comparer
    constructor(c?:any){
        if(c!=null){
            this.Default=new Comparer()
        }
    }
    Compare(...args):any{

    }
    default(a,b):any{

    }
    // Default(..args):any{
    //     return this.default(...args)
    // }
    numeric(a:any,b:any):any{

    }
    date(a:any,b:any):number{
        return 0
    }
   
}