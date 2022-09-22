using System.Collections.Generic;
using System;
using System.Diagnostics;
namespace FCSG{
    // public delegate bool BoolLinkedVariableDelegate(LinkedVariable<??> var);
    public delegate LinkedVariable[] LinkedVarListSpriteBaseDelegate(SpriteBase var);
    public delegate OutType ObjectSpriteBaseDelegate<OutType>(SpriteBase sb);
    
    public class LinkedVariable
    {
        internal bool updating = false; //If true, it means that the value is being updated (and has not yet been set)

        internal virtual void SetUpdating()
        {
            throw new NotImplementedException();
        }

        internal virtual void Update()
        {
            throw new NotImplementedException();
        }

        internal virtual void LinkTo(LinkedVariable lv)
        {
            throw new NotImplementedException();
        }

        internal virtual void AddLinkedVariable(LinkedVariable lv)
        {
            throw new NotImplementedException();
        }

        public object Get()
        {
            throw new NotImplementedException();
        }

        internal virtual void UnlinkFrom(LinkedVariable lv)
        {
            throw new NotImplementedException();
        }

        internal virtual void UnlinkTo(LinkedVariable lv)
        {
            throw new NotImplementedException();
        }
    }
    
    public class LinkedVariable<OutType>:LinkedVariable where OutType:struct{
        protected ObjectSpriteBaseDelegate<OutType> objectDelegate;
        protected SpriteBase spriteBase;
        protected List<LinkedVariable> linkedVariables=new List<LinkedVariable>(); //List of variables which will be updated when this variable is updated
        protected LinkedVarListSpriteBaseDelegate linkedVariablesDelegate=(SpriteBase sb)=>new LinkedVariable[] {}; //This is used to retrieve self-referencial values
        protected List<LinkedVariable> linkedFromVariables = new List<LinkedVariable>();

        protected OutType? lastValue = null; //The last valid value this linked variable had. Is used to avoid re-calculating Changed() and to avoid calling objectDelegate more than needed
        protected OutType? _value=null;
        protected OutType objectValue{
            get{
                if(_value==null){ //The first implementation would instantly calculate the value of the variable, leading to many nullPointer errors as the Sprite was not linked yet. This solves the errors by calculating the value the first time it is actually used.
                    _value=objectDelegate(spriteBase);
                }
                if(!updating){ //If the variable is not being updated, it returns the "static" value
                    return (OutType)_value;
                }
                else{ //If the variable is being updated, it updates the value and returns it.
                    // if(spriteBase!=null){
                    //     Console.WriteLine("Accessed delegate ("+objectDelegate(spriteBase)+"); sprite is "+spriteBase.name);
                    // }
                    _value=objectDelegate(spriteBase);//HACK: RANDOM ADDITION SEE IF THIS ACTUALLY WORKS->should be fine
                    updating=false;
                    return (OutType)_value;
                }
            }
            set{
                _value=value;
            }
        }

        #region Methods
        /// <summary>
        /// Set the value of the variable to the given value; if the value is different from the previous one, an update will be triggered.
        /// </summary>
        /// <param name="value">The value to be set</param>
        public void Set(OutType value){
            objectDelegate=(SpriteBase sb)=>value;
            if(Changed()){
                SetUpdating();
                Update();
            }
            else
            {
                updating = false;
            }
        }

        /// <summary>
        /// Get the current value of this linked variable
        /// </summary>
        /// <returns>The current value of this linked variable</returns>
        public new OutType Get()
        {
            return objectValue;
        }

        /// <summary>
        /// Set the sprite for this linkedVariable
        /// </summary>
        /// <param name="sprite">A sprite to be passed to the lambdas</param>
        public void SetSprite(SpriteBase sprite){
            this.spriteBase=sprite;
        }

        /// <summary>
        /// Sets updating to true for all the variables linked to this (and variables linked to variables linked to this, and so on)
        /// </summary>
        internal override void SetUpdating(){
            updating=true;
            foreach(LinkedVariable lv in linkedVariables){
                // Console.WriteLine("Setting "+sv+" to updating");
                if(!lv.updating){
                    // Console.WriteLine("Set "+sv+" to updating");
                    lv.SetUpdating();
                }
            }
        }

        /// <summary>
        /// Updates the value of _value and if it has changed, updates all variables linked to this one. IMPORTANT: this functions assumes <c>Changed()</c> is true.
        /// </summary>
        internal override void Update(){
            if (Changed())
            {
                _value = lastValue; //lastValue is the value calculated in Changed.
                updating = false;
                lastValue=null;
                // Console.WriteLine("SVariable updated: "+this.ToString());
                foreach (LinkedVariable lv in linkedVariables)
                {
                    if (lv.updating)
                    {
                        lv.Update();
                    }
                }
            }
        }

        /// <summary>
        /// Returns true if the value of the variable has changed since the last update.
        /// </summary>
        private bool Changed(){
            if(_value==null){
                return true;
            }else if (lastValue != null) //If lastValue is not null, it means that Changed has been called, but Update has not yet been called.
            {
                return true;
            }
            else
            {
                var specificEquals = typeof(OutType).GetMethod("Equals", new Type[] {typeof(OutType)});
                if (specificEquals != null && specificEquals.ReturnType==typeof(bool)) {
                    OutType currentValue = objectDelegate(spriteBase);
                    bool changed = !(bool)specificEquals.Invoke(_value, new object[] { currentValue });
                    if (changed)
                    {
                        lastValue = currentValue;
                    }

                    return changed;
                }
                // changed = _value.Equals(objectDelegate(this.spriteBase));
            }

            //Debug.WriteLine("Compared " + _value + " with " + objectDelegate(spriteBase)+" with result: "+changed);

            return false;
        }

        ///<summary>
        ///Adds a variable which will be updated when this variable is updated. This only links one way, which means that the link will not be automatically removed in case lv gets unlinked.
        ///</summary>
        internal override void AddLinkedVariable(LinkedVariable lv){//Adds a variable (lv) which is sensitive to this
            if(!linkedVariables.Contains(lv)){
                // Console.WriteLine("Adding "+this+" to "+sv);
                linkedVariables.Add(lv);
            }
        }
        ///<summary>
        ///Adds this variable to lv's linked variables, so that this is updated when lv is updated. This links both ways, which means that the link will be automatically removed in case lv gets unlinked.
        ///</summary>
        internal override void LinkTo(LinkedVariable lv){//Makes this variable sensitive to sv
            // Console.WriteLine("Adding "+this+" to "+sv);
            lv.AddLinkedVariable(this);
            if (!linkedFromVariables.Contains(lv))
            {
                linkedFromVariables.Add(lv);
            }
        }

        /// <summary>
        /// Unlinks all the variables which depend from this one, also removing the links in those variables which tell that they are linked from this one.
        /// </summary>
        public void UnlinkTo()
        {
            foreach(LinkedVariable lv in linkedVariables)
            {
                lv.UnlinkFrom(this);
            }
            linkedVariables = new List<LinkedVariable>();
        }

        /// <summary>
        /// If the current linked variables is linked to lv, this function removes that link.
        /// </summary>
        /// <param name="lv">The linked variable which "to link" is to be removed</param>
        internal override void UnlinkTo(LinkedVariable lv)
        {
            if (linkedVariables.Contains(lv))
            {
                linkedVariables.Remove(lv);
            }
        }

        /// <summary>
        /// Unlinks all the variables on which this one depends, also removing the links in those variables which tell that they are linked to this one.
        /// </summary>
        public void UnlinkFrom()
        {
            foreach(LinkedVariable lv in linkedFromVariables)
            {
                lv.UnlinkTo(lv);
            }
            linkedVariables=new List<LinkedVariable>();
        }

        /// <summary>
        /// If the current linked variables is linked from lv, this function removes that link.
        /// </summary>
        /// <param name="lv">The linked variable which "from link" is to be removed</param>
        internal override void UnlinkFrom(LinkedVariable lv)
        {
            if (linkedFromVariables.Contains(lv))
            {
                linkedFromVariables.Remove(lv);
            }
        }

        /// <summary>
        /// Unlinks all links from and to this variable. This function calls <c>UnlinkTo()</c> and <c>UnlinkFrom()</c>.
        /// </summary>
        public void Unlink()
        {
            UnlinkTo();
            UnlinkFrom();
        }


        #endregion Methods

        #region Constructors
        //Without anything
        public LinkedVariable(SpriteBase spriteBase, ObjectSpriteBaseDelegate<OutType> objectDelegate){
            this.spriteBase=spriteBase;
            // linkedVariables=new List<LinkedVariable>();
            this.objectDelegate=objectDelegate;
            // _value=objectDelegate(spriteBase);
            // FixValue();
        }

        //With arrays
        public LinkedVariable(SpriteBase spriteBase, ObjectSpriteBaseDelegate<OutType> objectDelegate, LinkedVariable[] sensitiveVariables){
            this.spriteBase=spriteBase;
            // linkedVariables=new List<LinkedVariable>();
            this.objectDelegate=objectDelegate;
            // _value=objectDelegate(spriteBase);
            // FixValue();
        }

        //Without spritebase, with arrays
        public LinkedVariable(ObjectSpriteBaseDelegate<OutType> objectDelegate, LinkedVariable[] sensitiveVariables){
            // linkedVariables=new List<LinkedVariable>();
            this.objectDelegate=objectDelegate;
            // _value=objectDelegate(spriteBase);
            foreach(LinkedVariable sv in sensitiveVariables){
                LinkTo(sv);
            }
        }

        //Without spritebase, without arrays
        public LinkedVariable(ObjectSpriteBaseDelegate<OutType> objectDelegate){
            // linkedVariables=new List<LinkedVariable>();
            this.objectDelegate=objectDelegate;
            // _value=objectDelegate(spriteBase);
            // FixValue();
        }

        //With settings
        /// <summary>
        /// Creates a LinkedVariable with the given settings; the variable will then need to be activated using Activate() in order for variables to link correctly
        /// </summary>
        public LinkedVariable(SpriteBase spriteBase, LinkedVariableParams<OutType> parameters){
            this.spriteBase=spriteBase;
            // linkedVariables=new List<LinkedVariable>();
            this.objectDelegate=parameters.objectDelegate;
            // _value=objectDelegate(spriteBase);
            // FixValue();
            if(parameters.sensitiveVariables!=null){
                this.linkedVariablesDelegate=(SpriteBase sb)=>parameters.sensitiveVariables;
            }else if(parameters.linkedVariableDelegate!=null){
                this.linkedVariablesDelegate=parameters.linkedVariableDelegate;
            }
        }

        //With settings but without SpriteBase
        /// <summary>
        /// Creates a LinkedVariable with the given settings; the variable will then need to be activated using Activate(SpriteBase) in order for variables to link correctly
        /// </summary>
        public LinkedVariable(LinkedVariableParams<OutType> parameters){
            // linkedVariables=new List<LinkedVariable>();
            this.objectDelegate=parameters.objectDelegate;
            // _value=objectDelegate(spriteBase);
            // FixValue();
            if(parameters.sensitiveVariables!=null){
                this.linkedVariablesDelegate=(SpriteBase sb)=>parameters.sensitiveVariables;
            }else if(parameters.linkedVariableDelegate!=null){
                this.linkedVariablesDelegate=parameters.linkedVariableDelegate;
            }
        }

        /// <summary>
        /// Constructs a linked variable which will only hold the static value of the given object, and will only be updated through <c>LinkedVariable.Set()</c>
        /// </summary>
        /// <param name="value">The value of this linked variable</param>
        public LinkedVariable(OutType value)
        {
            this.objectDelegate = (SpriteBase sb) => value;
        }
        #endregion Constructors

        ///<summary>
        ///Links this to the LinkedVariables it should be linked to. Is used to link everything once it is sure that values are not null. It uses the linkedVariablesDelegate to get the linked variables
        ///</summary>
        public void Activate(){
            foreach(LinkedVariable sv in linkedVariablesDelegate(spriteBase)){
                LinkTo(sv);
            }
        }

        ///<summary>
        ///Links this to the LinkedVariables it should be linked to. Is used to link everything once it is sure that values are not null. It uses the linkedVariablesDelegate to get the linked. This overload of activate also links a new SpriteBase.
        ///</summary>
        public void Activate(SpriteBase sb){
            spriteBase=sb;
            foreach(LinkedVariable lv in linkedVariablesDelegate(spriteBase)){
                LinkTo(lv);
            }
        }

        public override string ToString(){
            return "lv["+objectValue.ToString()+"]";
        }
    }

    public class LinkedVariableParams<OutType>{
        public ObjectSpriteBaseDelegate<OutType> objectDelegate;
        public LinkedVariable[] sensitiveVariables;
        public LinkedVarListSpriteBaseDelegate linkedVariableDelegate;

        public LinkedVariableParams(ObjectSpriteBaseDelegate<OutType> objectDelegate, LinkedVariable[] sensitiveVariables=null, LinkedVarListSpriteBaseDelegate sensitiveDelegate=null){
            this.objectDelegate=objectDelegate;
            this.sensitiveVariables=sensitiveVariables;
            this.linkedVariableDelegate=sensitiveDelegate;
        }

        public LinkedVariableParams(OutType value)
        {
            objectDelegate=(SpriteBase sb) => value;
            sensitiveVariables = null;
            linkedVariableDelegate=null;
        }
    }
}