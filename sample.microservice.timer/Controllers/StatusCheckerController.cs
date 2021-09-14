using System;
using System.Threading;
using sample.microservice.useraccount.Controllers;

namespace StatusCheckerController
{
    class StatusChecker
    {
        public static void CheckTempStatus(object o)
        {   
            AutoResetEvent autoEvent = (AutoResetEvent)o;
            Random rnd = new Random();
            int _highTemp = 30;
            int _maxTemp = 95;

            int _currentTemp = rnd.Next(15,100);
            if(_currentTemp < _highTemp){
                Console.WriteLine("{0:h:mm:ss.fff} Current Temperature {1}*C.\n", DateTime.Now, _currentTemp);}
            else if(_currentTemp > _highTemp && _currentTemp < _maxTemp){
                Console.WriteLine("{0:h:mm:ss.fff} Current Temperature {1}*C.\nWarning High Temperature!\n", DateTime.Now, _currentTemp);}
            else if(_currentTemp > _maxTemp){
                Console.WriteLine("{0:h:mm:ss.fff} Current Temperature {1}*C.\nThread Ended! Temperature above {2}*C.\n", DateTime.Now, _currentTemp, _maxTemp);
                autoEvent.Set();} 
        }

        public static void CheckHeightStatus(object o)
        {   
            AutoResetEvent autoEvent = (AutoResetEvent)o;
            Random rnd = new Random();
            int _maxHeight = 45;

            int _currentHeight = rnd.Next(15,50);
            if(_currentHeight < _maxHeight){
                Console.WriteLine("{0:h:mm:ss.fff} Current Height {1}mm.\n", DateTime.Now, _currentHeight);}
            else if(_currentHeight > _maxHeight){
                Console.WriteLine("{0:h:mm:ss.fff} Current Height {1}mm.\nThread Ended! Height above {2}mm.\n", DateTime.Now, _currentHeight, _maxHeight);
                autoEvent.Set();}
        }
    }
}