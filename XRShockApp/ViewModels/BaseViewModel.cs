using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace XRShock.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged, INotifyCollectionChanged, INotifyDataErrorInfo
    {
        #region INotifyPropertyChanged
        /// <summary>
        /// PropertyChangedEvent
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// CollectionChangedExent
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// OnPropertyChanged
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// OnCollectionChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="action"></param>
        /// <param name="value"></param>
        protected virtual void OnCollectionChanged(object sender, NotifyCollectionChangedAction action, object value)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, value));
        }
        #endregion


        #region INotifyDataErrorInfo

        private Dictionary<string, List<string>> propNameToErrorsDict = new Dictionary<string, List<string>>();

        /// <summary>
        /// ErrorsChangedExent
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// HasErrors
        /// </summary>
        public bool HasErrors => propNameToErrorsDict.Any();

        /// <summary>
        /// GetErrors for given property
        /// </summary>  
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public IEnumerable GetErrors(string propertyName)
        {
            return propNameToErrorsDict.ContainsKey(propertyName) ?
                propNameToErrorsDict[propertyName] : null;
        }

        /// <summary>
        /// Checks if given property has errors
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public bool PropertyHasErrors(string propertyName)
        {
            if (propNameToErrorsDict.ContainsKey(propertyName))
                return true;
            else
                return false;
        }


        /// <summary>
        /// GetErrorDict returns propNameToErrorsDict
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllErrors()
        {
            List<string> errorList = new List<string>();

            foreach (KeyValuePair<string, List<string>> errorKVP in propNameToErrorsDict)
            {
                foreach (var errors in errorKVP.Value)
                {
                    errorList.Add(errors);
                }
            }
            return errorList;
        }

        /// <summary>
        /// OnErrorsChanged
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnErrorsChanged([CallerMemberName] string propertyName = null)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Adds an error in the error dict 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="error"></param>
        public void AddError(string propertyName, string error)
        {
            if (!propNameToErrorsDict.ContainsKey(propertyName))
                propNameToErrorsDict[propertyName] = new List<string>();

            if (!propNameToErrorsDict[propertyName].Contains(error))
            {
                propNameToErrorsDict[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        /// <summary>
        /// clears an errorDict entry for given propertyName
        /// </summary>
        /// <param name="propertyName"></param>
        public void ClearErrors(string propertyName)
        {
            if (propNameToErrorsDict.ContainsKey(propertyName))
            {
                propNameToErrorsDict.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }

        #endregion
    }
}
