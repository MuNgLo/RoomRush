using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum ROOMSTATE { PRE, ACTIVE, POST }
public enum PLAYERSTATE { DEAD, ALIVE } // Dead is used outside runs when starting the game or run ends
public enum ROOMEVENTS { CLEAR, PARTIMEOUT, FAIL }
